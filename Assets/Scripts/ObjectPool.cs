using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private readonly List<GameObject> _pool = new();

    protected int Capacity;

    protected void InitPoolObject(GameObject prefab)
    {
        for(int i = 0; i < Capacity; i++)
        {
            CreateObject(prefab); // , i == 0 ? true : false
        }
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false);
        return result != null;
    }

    private void CreateObject(GameObject prefab, bool isActiveByDefault = false)
    {
        GameObject spawned = Instantiate(prefab, _container);
        spawned.SetActive(isActiveByDefault);
        _pool.Add(spawned);
    }

    protected void RestartPool()
    {
        foreach(GameObject pool in _pool)
        {
            pool.SetActive(false);
        }
    }
}