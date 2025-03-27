using System;
using Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] private float lifetime = 100f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private int damage = 10;
    
        [Inject] private PlayerStatsController _playerStatsController;

        private Transform _player;
        private Rigidbody2D _rigidbody;
        private IMemoryPool _pool;
        private IDisposable _attackDisposable;
        private bool _isDespawned;

        private float toleranceFlip = 0.3f;
        private int health;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>() ?? throw new NullReferenceException("Rigidbody2D is missing on Enemy");
        }

        public void Initialize(Transform player, IMemoryPool pool)
        {
            _player = player;
            _pool = pool;
            _isDespawned = false;

            health = maxHealth;
        }

        private void Start()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => _player != null)
                .Subscribe(_ => MoveTowardsPlayer())
                .AddTo(this);
        }

        private void MoveTowardsPlayer()
        {
            if (_player == null) return;

            Vector3 direction = (_player.position - transform.position).normalized;
            _rigidbody.velocity = direction * speed;

            Flip(_player.position.x);
        }
    
        private void Flip(float enemyX)
        {
            if (Mathf.Abs(enemyX - transform.position.x) <= toleranceFlip) return;
        
            float initialScaleX = Mathf.Abs(transform.localScale.x); 
            int shouldFlip = enemyX < transform.position.x ?  -1 : 1;
            Vector3 localScale = transform.localScale;
            localScale.x = initialScaleX * shouldFlip;
            transform.localScale = localScale;
        }
    
        private void AttackPlayer()
        {
            if (_player == null || _isDespawned) return;
        
            _playerStatsController.TakeDamage(damage);
        }
    
        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health > 0) return;
        
            _playerStatsController.AddScore(1);
            _playerStatsController.AddExperience(2);
            Despawn();
        }

        private void Despawn()
        {
            if (_isDespawned) return;
            _isDespawned = true;
            _attackDisposable?.Dispose();
            _pool.Despawn(this);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.transform.CompareTag("Player") || !gameObject.activeSelf) return;
        
            _attackDisposable?.Dispose();
        
            AttackPlayer();
        
            _attackDisposable = Observable.Interval(TimeSpan.FromSeconds(attackCooldown))
                .Subscribe(_ => AttackPlayer())
                .AddTo(this);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.transform.CompareTag("Player") || !gameObject.activeSelf) return;
        
            _attackDisposable?.Dispose();
        }

        private void OnDestroy()
        {
            _attackDisposable?.Dispose();
        }
    }
}