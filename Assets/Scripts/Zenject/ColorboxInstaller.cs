using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class ColorboxInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridNodeInformation>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AIDestinationSetterCustom>().FromComponentInHierarchy().AsTransient();
        //Container.BindInterfacesAndSelfTo<IInputSystem>().AsSingle();
        
        Container.Bind<NavMeshAgent>().FromComponentSibling().AsTransient();

        SignalBusInstaller.Install(Container);
        
        //input system scripts signals
        Container.DeclareSignal<ColorBoxSignals.SendNewDestinationToAiSignal>();
        Container.DeclareSignal<ColorBoxSignals.SendNodeInformationToNeighborStatusSignal>();

    }
    
    
}