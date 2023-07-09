using UnityEngine;
using Zenject;

public class HandleSelectorInstaller : MonoInstaller
{
    [SerializeField] private HandleSelector _selector;

    public override void InstallBindings()
    {
        Container.Bind<HandleSelector>().FromInstance(_selector).AsSingle().NonLazy();
    }
}
