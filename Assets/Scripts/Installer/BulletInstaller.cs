using UnityEngine;
using Weapon;
using Zenject;

namespace Installer
{
    public class BulletInstaller : MonoInstaller
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        public override void InstallBindings()
        {
            Container.BindFactory<Vector2, Bullet, BulletFactory>().AsSingle();
        
            Container.BindMemoryPool<Bullet, BulletPool>().WithInitialSize(10)
                .FromComponentInNewPrefab(bulletPrefab)
                .UnderTransformGroup("Bullets");
        }
    }
}
