using UnityEngine;
using Zenject;

public class FurnitureFactory : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;

    public GameObject CreateFurniture(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return _container.InstantiatePrefab(prefab, position, rotation, null);
    }
}
