using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 40;
        [SerializeField] private float lifetime = 3f;
    
        private const string ENEMY_TAG = "Enemy";
    
        private Rigidbody2D _rigidbody;
        private IMemoryPool _pool;
        private IDisposable _lifetimeDisposable;

        private Animator _animator;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>() ?? throw new System.NullReferenceException("Rigidbody2D is missing on Bullet");
            _animator = GetComponent<Animator>() ?? throw new System.NullReferenceException("Animator is missing on Bullet");
        }

        public void Initialize(Vector2 direction, IMemoryPool pool, Vector3 position)
        {
            _pool = pool;
            transform.position = position;
            gameObject.SetActive(true);
        
            Vector2 normalizedDirection = direction.normalized;
            float angle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.y) * Mathf.Rad2Deg;
        
            _rigidbody.velocity = normalizedDirection * speed;
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        
            _lifetimeDisposable = Observable.Timer(System.TimeSpan.FromSeconds(lifetime))
                .Subscribe(_ => _pool.Despawn(this))
                .AddTo(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(ENEMY_TAG) || !gameObject.activeSelf) return;
        
            if (!other.TryGetComponent<Enemy.Enemy>(out var enemy)) return;
        
            enemy.TakeDamage(damage);
            _rigidbody.velocity = Vector2.zero;
            _animator.Play("Hit");
        }

        public void Despawn()
        {
            _lifetimeDisposable?.Dispose();
            _pool.Despawn(this);
        }
    
        public void OnDespawned()
        {
            _lifetimeDisposable?.Dispose();
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }
    }
}