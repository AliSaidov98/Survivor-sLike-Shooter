using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public class BootManager : MonoBehaviour
    {
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        }

        private void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(_ => _sceneLoader.LoadSceneAsync("Gameplay").Subscribe(),
                    ex => Debug.LogError($"BootManager: Error loading scene - {ex.Message}"))
                .AddTo(this);
        }
    }
}