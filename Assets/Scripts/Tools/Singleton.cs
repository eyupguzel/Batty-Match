using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("Instance is null!");

            return instance;
        }
    }
    protected void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            Init();
        }
        else
        {
            Destroy(instance);
        }
    }
    protected void OnDestroy()
    {
        if (instance != null)
            instance = null;
    }
    protected virtual void Init() { }
}
