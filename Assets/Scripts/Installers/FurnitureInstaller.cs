using UnityEngine;
using Zenject;

public class FurnitureInstaller : MonoInstaller
{
    [SerializeField] private FurnitureFactory _furnitureFactory;

    [SerializeField] private Material[] _transperentMaterial;
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField] private LayerMask _wallsLayer;
    [SerializeField] private ParticleSystem _buyFX;

    public override void InstallBindings()
    {
        Container.Bind<FurniturePlacemant>()
            .FromMethod((ctx) => new FurniturePlacemant(_furnitureFactory, _floorLayer, _wallsLayer, _transperentMaterial, _buyFX))
            .AsSingle();

        Container.Bind<FurnitureFactory>().FromInstance(_furnitureFactory).AsSingle();
    }
}
