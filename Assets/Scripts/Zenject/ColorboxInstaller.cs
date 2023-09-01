using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class ColorboxInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridGenerator>().FromComponentInHierarchy().AsSingle();
        
        //Container.BindInterfacesAndSelfTo<IInputSystem>().AsSingle();
        
        Container.Bind<NavMeshAgent>().FromComponentSibling().AsTransient();
        //Container.Bind<NavMeshSurface>().FromComponentInHierarchy().AsSingle();

        SignalBusInstaller.Install(Container);
        
        //input system scripts signals
<<<<<<< Updated upstream
        Container.DeclareSignal<ColorBoxSignals.SendNewDestinationToAiSignal>();
=======
        Container.DeclareSignal<ColorBoxSignals.SelectedDestination>();
        Container.DeclareSignal<ColorBoxSignals.AgentReachedTargetNode>();
>>>>>>> Stashed changes

    }
}