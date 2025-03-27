using SimpleInputNamespace;
using UniRx;
using UnityEngine;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
    
        [Inject] private Joystick _joystick;
    
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
                throw new MissingComponentException("Отсутствует Rigidbody2D у Player.");
        }

        private void Start()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => _rigidbody != null)
                .Subscribe(_ =>
                {
                    Vector2 moveInput = new Vector2(_joystick.xAxis.value, _joystick.yAxis.value);
                    _rigidbody.velocity = moveInput.normalized * _speed;
                })
                .AddTo(this);
        }
    }
}