using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private List<Enemy.Enemy> enemyPrefabs;
    
        public override void InstallBindings()
        {

            foreach (var enemyPrefab in enemyPrefabs)
            {
                Container.BindMemoryPool<Enemy.Enemy, EnemyPool>()
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(enemyPrefab)
                    .UnderTransformGroup("Enemies");
            }
        }
    }
}
