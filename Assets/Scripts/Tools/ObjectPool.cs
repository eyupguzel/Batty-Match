using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : Singleton<ObjectPool<T>> where T : MonoBehaviour
{
    [SerializeField] protected T prefab;

    private List<T> pooledObjects;
    private int amount;
    private bool isReady;

    public void ObjectsPool(int amount = 0)
    {
        if (amount < 0)
            throw new System.ArgumentException("Amount to pool must be non-negative !");

        this.amount = amount;
        pooledObjects = new List<T>(amount);

        GameObject newObject;

        for (int i = 0; i != amount; i++)
        {
            newObject = Instantiate(prefab.gameObject, transform);
            newObject.SetActive(false);
            pooledObjects.Add(newObject.GetComponent<T>());
        }
        isReady = true;
    }

    public T GetObjectPooled()
    {
        if (!isReady)
            ObjectsPool(1);

        for (int i = 0; i != amount; i++)
            if (!pooledObjects[i].isActiveAndEnabled)
                return pooledObjects[i];

        GameObject newObject = Instantiate(prefab.gameObject, transform);
        newObject.SetActive(true);
        pooledObjects.Add(newObject.GetComponent<T>());
        ++amount;

        return newObject.GetComponent<T>();
    }
    public void ReturnObjectToPool(T toBeReturned)
    {
        if (toBeReturned == null)
            return;
        if (!isReady)
        {
            ObjectsPool();
            pooledObjects.Add(toBeReturned);
        }
        toBeReturned.gameObject.SetActive(false);
    }
}
