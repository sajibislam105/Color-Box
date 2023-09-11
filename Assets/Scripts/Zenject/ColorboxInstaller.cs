using UnityEngine;
using UnityEngine.AI;

namespace Zenject
{
    public class ColorboxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GridNodeInformation>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AIDestinationSetterCustom>().FromComponentInHierarchy().AsTransient();
            Container.Bind<ClaimAnimation>().FromComponentInHierarchy().AsSingle();
            //Container.BindInterfacesAndSelfTo<IInputSystem>().AsSingle();
        
            Container.Bind<NavMeshAgent>().FromComponentSibling().AsTransient();

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ColorBoxSignals.SelectedDestination>();//input system scripts signal
            Container.DeclareSignal<ColorBoxSignals.AgentReachedTargetNode>();//Destination Check Signal
            Container.DeclareSignal<ColorBoxSignals.AgentSelectionStatus>();//neighbor check signal
            Container.DeclareSignal<ColorBoxSignals.NodeSelection>();
            Container.DeclareSignal<ColorBoxSignals.WalkingAnimationSignal>(); //Animation Signal
            
            //UI Signals
            Container.DeclareSignal<ColorBoxSignals.LoadEverything>();
            Container.DeclareSignal<ColorBoxSignals.FirstTappedLevelStart>();
            Container.DeclareSignal<ColorBoxSignals.LevelComplete>();
            Container.DeclareSignal<ColorBoxSignals.LevelFailed>();
            Container.DeclareSignal<ColorBoxSignals.CompletionProgressBarSignal>();
            Container.DeclareSignal<ColorBoxSignals.MoveCounter>();
            Container.DeclareSignal<ColorBoxSignals.RemainingMoves>();
            Container.DeclareSignal<ColorBoxSignals.CoupleMergeCount>();
            Container.DeclareSignal<ColorBoxSignals.CoinEarned>();
            Container.DeclareSignal<ColorBoxSignals.ClaimedAndCoinAddedToBalance>();
            
        }
    }
}