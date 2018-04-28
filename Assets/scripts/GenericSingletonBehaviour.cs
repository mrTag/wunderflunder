using UnityEngine;

public abstract class GenericSingletonBehaviour<SingletonClassType> : MonoBehaviour
    where SingletonClassType : GenericSingletonBehaviour<SingletonClassType>
{
    public abstract string GetName();

    private static volatile SingletonClassType instance;

    public static SingletonClassType Instance
    {
        get
        {
            return CreateInstance();
        }
    }

    public static SingletonClassType CreateInstance()
    {
        if (instance == null)
        {
            Debug.LogWarning("Creating new instance of Singleton for " + typeof(SingletonClassType) + "! That is probably not what you intended!");
            GameObject obj = new GameObject();
            instance = obj.AddComponent<SingletonClassType>();
            if (instance != null) obj.name = instance.GetName();
        }
        return instance;
    }

    public static bool hasInstance
    {
        get
        {
            return instance != null;
        }
    }

    protected void SetInstanceRefToNull()
    {
        instance = null;
    }

    protected void ReplaceInstance(SingletonClassType newInstance)
    {
        instance = newInstance;
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (SingletonClassType)this;
        }
        else if (instance != this)
        {
            Component[] cmp = GetComponents<Component>();
            if (cmp.Length > 2)
                Destroy(this);
            else
                Destroy(gameObject);
        }
    }
}