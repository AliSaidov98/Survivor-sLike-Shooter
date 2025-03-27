using System.Linq;
using Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform weaponPivot;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float aimRadius = 5f;
    
        [Inject] private BulletFactory _bulletFactory;
        [Inject] private PlayerMovement _player;
    
        private const string ENEMY_TAG = "Enemy";
    
        private float _nextFireTime;
        private Transform target;
        private float toleranceFlip = 0.3f;


        private void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Time.time >= _nextFireTime)
                .Subscribe(_ => TryShoot())
                .AddTo(this);
        
            Observable.EveryUpdate()
                .Where(_ => target != null)
                .Subscribe(_ => LookTowardsTarget())
                .AddTo(this);
        }

        private void TryShoot()
        {
            target = FindClosestEnemy();
            if (target == null) return;
        
            Vector2 direction = (target.position - transform.position).normalized;
            Fire(direction);

            _nextFireTime = Time.time + fireRate;
        }
    
        private Transform FindClosestEnemy()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aimRadius);
            return hits
                .Where(c => c.CompareTag(ENEMY_TAG))
                .OrderBy(c => (c.transform.position - transform.position).sqrMagnitude)
                .Select(c => c.transform)
                .FirstOrDefault();
        }

        private void Fire(Vector2 direction)
        {
            _bulletFactory.Create(direction, firePoint);
        }

        private void LookTowardsTarget()
        {
            Vector2 direction = (target.position - transform.position).normalized;

            FlipPlayer(target.position.x);
            RotateWeapon(direction);
        }
    
    
        private void FlipPlayer(float enemyX)
        {
            if (Mathf.Abs(enemyX - transform.position.x) <= toleranceFlip) return;
            
            int shouldFlip = enemyX < transform.position.x ?  -1 : 1;
            Vector3 localScale = _player.transform.localScale;
            localScale.x = shouldFlip;
            _player.transform.localScale = localScale;
        
            Vector3 pivotScale = weaponPivot.localScale;
            pivotScale.x = shouldFlip;
            pivotScale.y = shouldFlip;
            weaponPivot.localScale = pivotScale;
        }
    
        private void RotateWeapon(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
        }
    
    }
}