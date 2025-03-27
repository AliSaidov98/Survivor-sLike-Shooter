using UnityEngine;
using Zenject;

namespace Weapon
{
    public class BulletFactory : PlaceholderFactory<Vector2, Bullet>
    {
        private readonly BulletPool _pool;

        [Inject]
        public BulletFactory(BulletPool pool)
        {
            _pool = pool;
        }

        public Bullet Create(Vector2 direction, Transform firePoint)
        {
            Bullet bullet = _pool.Spawn();
            bullet.Initialize(direction, _pool, firePoint.position);
            return bullet;
        }
    }
}