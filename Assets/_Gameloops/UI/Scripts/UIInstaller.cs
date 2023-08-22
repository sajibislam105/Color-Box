using Gameloops;
using Gameloops.UI;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<UIUtils>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ResourceView>().FromComponentInHierarchy().AsSingle();
    }
}