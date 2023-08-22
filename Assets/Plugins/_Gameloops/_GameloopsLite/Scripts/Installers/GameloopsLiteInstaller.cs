using Gameloops;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameloopsLiteInstaller", menuName = "Installers/GameloopsLiteInstaller")]
public class GameloopsLiteInstaller : ScriptableObjectInstaller<GameloopsLiteInstaller>
{
    public GameSettings gameSettings;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.BindInterfacesAndSelfTo<AnalyticsManager>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<StorageManager>().FromNew().AsSingle();

        Container.BindInstance(gameSettings);
        
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();

        Container.DeclareSignal<GameSignals.LevelStartedSignal>();
        Container.DeclareSignal<GameSignals.LevelCompletedSignal>();
        Container.DeclareSignal<GameSignals.LevelLoadedSignal>();
        Container.DeclareSignal<GameSignals.LevelFailedSignal>();
        Container.DeclareSignal<GameSignals.LevelLoadSameSignal>();
        Container.DeclareSignal<GameSignals.LevelLoadNextSignal>();
        Container.DeclareSignal<GameSignals.ProgressUpdatedSignal>();
    }
}