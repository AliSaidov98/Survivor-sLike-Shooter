using UniRx;

namespace Player
{
    public class PlayerStatsModel
    {
        public ReactiveProperty<int> Health { get; } = new ReactiveProperty<int>(100);
        public ReactiveProperty<int> Score { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> Level { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> Experience { get; } = new ReactiveProperty<int>(0);

    
        public void TakeDamage(int damage)
        {
            if(Health.Value <= 0 || damage < 0) return;
        
            Health.Value -= damage;
        }

        public void AddScore(int amount)
        {
            Score.Value += amount;
        }
    
        public void AddLevel(int amount)
        {
            if(amount < 0) return;
        
            Level.Value += amount;
            Experience.Value = 0;
        }
    
        public void AddExperience(int amount)
        {
            if(amount < 0) return;
        
            Experience.Value += amount;
        }
    }
}
