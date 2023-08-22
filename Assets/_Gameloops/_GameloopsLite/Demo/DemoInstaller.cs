using Gameloops.Player;
using UnityEngine;
using Zenject;

public class DemoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerResource>().FromComponentInHierarchy().AsSingle();
    }
}