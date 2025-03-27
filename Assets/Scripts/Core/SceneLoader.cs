using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader
    {
        public IObservable<Unit> LoadSceneAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("SceneLoader: sceneName is null or empty!");
                return Observable.Empty<Unit>();
            }

            return Observable.Create<Unit>(observer =>
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                if (asyncLoad == null)
                {
                    Debug.LogError($"SceneLoader: Failed to load scene {sceneName}");
                    observer.OnError(new Exception($"Scene {sceneName} failed to load"));
                    return Disposable.Empty;
                }

                asyncLoad.completed += _ =>
                {
                    observer.OnNext(Unit.Default);
                    observer.OnCompleted();
                };

                return Disposable.Empty;
            });
        }

        public void RestartScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            LoadSceneAsync(activeScene.name).Subscribe();
        }
    }
}