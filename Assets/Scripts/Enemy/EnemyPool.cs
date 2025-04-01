using Zenject;

namespace Enemy
{
    public class EnemyPool : MonoMemoryPool<Enemy>
    {
        protected override void OnCreated(global::Enemy.Enemy item) => item.gameObject.SetActive(false);
        protected override void OnSpawned(global::Enemy.Enemy item) => item.gameObject.SetActive(true);
        protected override void OnDespawned(global::Enemy.Enemy item) => item.gameObject.SetActive(false);

    }
}
