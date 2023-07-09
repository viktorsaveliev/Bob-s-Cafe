using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected Transform Container;

    protected readonly List<GameObject> Pool = new();

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
        result = Pool.FirstOrDefault(p => p.activeSelf == false);
        return result != null;
    }

    protected virtual void CreateObject(GameObject prefab, bool isActiveByDefault = false)
    {
        GameObject spawned = Instantiate(prefab, Container);
        spawned.SetActive(isActiveByDefault);
        Pool.Add(spawned);
    }

    protected void RestartPool()
    {
        foreach(GameObject pool in Pool)
        {
            pool.SetActive(false);
        }
    }
}