using Core;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        private PlayerStatsModel _playerModel;
        private PlayerStatsView _playerView;

        private int maxPlayerExperience = 100;
        private int playerExperience;
        private int playerLvl;
    
        [Inject] private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(PlayerStatsModel playerModel, PlayerStatsView playerView)
        {
            _playerModel = playerModel;
            _playerView = playerView;

            _playerView.Bind(_playerModel);
        }

        public void TakeDamage(int damage)
        {
            _playerModel.TakeDamage(damage);
        
            if (_playerModel.Health.Value <= 0)
            {
                _sceneLoader.RestartScene();
            }
        }

        public void AddScore(int amount)
        {
            _playerModel.AddScore(amount);
        }
    
        public void AddLevel(int amount)
        {
            _playerModel.AddLevel(amount);
        }
    
        public void AddExperience(int amount)
        {
            _playerModel.AddExperience(amount);

            playerExperience += amount;
        
            if (playerExperience >= maxPlayerExperience)
            {
                playerExperience = 0;
                AddLevel(1);
            }
        }
    }
}
