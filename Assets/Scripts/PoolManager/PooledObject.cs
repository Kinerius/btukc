using UnityEngine;


public abstract class PooledObject : MonoBehaviour
{
    private Pool pool;

    public void SetupPool(Pool pool)
    {
        this.pool = pool;
    }

    public virtual void OnSpawn()
    {
        
    }

    public virtual void ReturnToPool()
    {
        pool.Return(this);
    }
}
