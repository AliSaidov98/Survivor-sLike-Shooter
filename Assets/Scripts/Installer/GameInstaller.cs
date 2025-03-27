using Core;
using Enemy;
using Player;
using SimpleInputNamespace;
using Zenject;

namespace Installer
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
        
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle(); 
            Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Joystick>().FromComponentInHierarchy().AsSingle();
        
            Container.Bind<PlayerStatsModel>().AsSingle();
            Container.Bind<PlayerStatsView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerStatsController>().FromComponentInHierarchy().AsSingle();
        }
    }
}