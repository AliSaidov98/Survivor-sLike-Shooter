using System;
using System.Collections.Generic;
using Player;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 2f;
        [SerializeField] private float spawnDistance = 10f;
        [SerializeField] private int maxEnemies = 10;
    
        private Transform _player;
        private CompositeDisposable _disposables = new CompositeDisposable();
    
        private List<EnemyPool> _enemyPools;
    
        [Inject]
        private void Construct(PlayerMovement player, List<EnemyPool> enemyPools)
        {
            _player = player.transform;
            _enemyPools = enemyPools;

            Observable.Interval(TimeSpan.FromSeconds(spawnRate))
                .Where(_ => GetActiveEnemiesCount() < maxEnemies)
                .Subscribe(_ => SpawnEnemy())
                .AddTo(_disposables);
        }

        private void SpawnEnemy()
        {
            if (_enemyPools.Count == 0 || _player == null) return;

            Vector2 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPosition = _player.position + new Vector3(spawnDirection.x, spawnDirection.y, 0);

            EnemyPool selectedPool = _enemyPools[Random.Range(0, _enemyPools.Count)];
            global::Enemy.Enemy enemyInstance = selectedPool.Spawn();
            enemyInstance.transform.position = spawnPosition;
            enemyInstance.Initialize(_player, selectedPool);
        }

        private int GetActiveEnemiesCount()
        {
            int count = 0;
        
            foreach (var pool in _enemyPools)
            {
                count += pool.NumActive;
            }
        
            return count;
        }
    
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}