using Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public class CameraFollow : MonoBehaviour
    {

        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector3 offset;

        [Inject] private PlayerMovement _player;
        
        private void Start()
        {
            if (_player == null)
            {
                Debug.LogError("CameraFollow: PlayerMovement is not injected!");
                return;
            }
        
            Observable.EveryFixedUpdate()
                .Where(_ => _player != null)
                .Subscribe(_ =>
                {
                    Vector3 targetPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                })
                .AddTo(this);
        }
    }
}