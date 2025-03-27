using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStatsView : MonoBehaviour
    {
        [SerializeField] private Text killText;
        [SerializeField] private Slider health;
        [SerializeField] private Slider experience;
        [SerializeField] private Text lvlText;

        public void Bind(PlayerStatsModel model)
        {
            model.Score.Subscribe(value => killText.text = $"{value}").AddTo(this);
            model.Health.Subscribe(value => health.value = (float)value * 0.01f).AddTo(this);
            model.Experience.Subscribe(value => experience.value = (float)value * 0.01f).AddTo(this);
            model.Level.Subscribe(value => lvlText.text = $"{value}").AddTo(this);
        }
    }
}
