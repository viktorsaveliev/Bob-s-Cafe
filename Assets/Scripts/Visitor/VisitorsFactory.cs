using UnityEngine;
using Zenject;

public class VisitorsFactory : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;

    public GameObject CreateVisitor(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        return _container.InstantiatePrefab(prefab, position, rotation, parent);
    }
}
