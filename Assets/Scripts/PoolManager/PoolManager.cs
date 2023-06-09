﻿using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolManager : IPoolManager
{
    private readonly Dictionary<PooledObject, Pool> pools = new();
    private readonly GameObject root;

    public PoolManager()
    {
        root = new GameObject("_PoolManager")
        {
            transform =
            {
                position = Vector3.one * 9999,
            },
        };
    }

    public T GetInactiveObject<T>(T reference) where T : PooledObject
    {
        if (reference == null)
        {
            Debug.LogError("You are trying to spawn a null object!");
            return null;
        }

        if (!pools.ContainsKey(reference))
        {
            var poolRoot = new GameObject("Pool-" + reference.name);
            poolRoot.transform.SetParent(root.transform);
            pools[reference] = new Pool(() => CreateInstance(reference, poolRoot.transform), poolRoot.transform);
        }

        var pool = pools[reference];
        var obj = pool.GetInactiveObject();
        obj.OnSpawn();
        return obj.GetComponent<T>();
    }

    private PooledObject CreateInstance(PooledObject reference, Transform poolRootTransform)
    {
        var obj = Object.Instantiate(reference, poolRootTransform);
        obj.gameObject.SetActive(false);
        return obj;
    }
}

public interface IPoolManager
{
    T GetInactiveObject<T>(T reference) where T: PooledObject;
}
