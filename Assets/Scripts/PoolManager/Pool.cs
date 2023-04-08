using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Queue<PooledObject> poolQueue = new Queue<PooledObject>();
    private Func<PooledObject> constructor;
    private Transform root;

    public Pool(Func<PooledObject> constructor, Transform root)
    {
        this.root = root;
        this.constructor = constructor;
    }

    public PooledObject GetInactiveObject()
    {
        if (poolQueue.Count == 0)
        {
            var pooledObject = constructor();
            pooledObject.SetupPool(this);
            pooledObject.gameObject.SetActive(false);
            poolQueue.Enqueue(pooledObject);
        }

        return poolQueue.Dequeue();
    }

    public void Return(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.SetParent(root);
        poolQueue.Enqueue(pooledObject);
    }
}