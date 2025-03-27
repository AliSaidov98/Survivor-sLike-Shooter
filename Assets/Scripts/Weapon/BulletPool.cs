using Zenject;

namespace Weapon
{
    public class BulletPool : MonoMemoryPool<Bullet>
    {
        protected override void OnDespawned(Bullet item)
        {
            item.gameObject.SetActive(false);
        }
    }
}
