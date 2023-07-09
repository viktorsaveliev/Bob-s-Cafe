using UnityEngine;
using Zenject;

public class VisualDataConfigInstaller : MonoInstaller
{
    [SerializeField] private VisualDataConfig _visualDataConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(_visualDataConfig).AsSingle();
    }
}
