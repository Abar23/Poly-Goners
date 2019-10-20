using UnityEngine;

public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    private static bool applicationIsQuitting = false;

    public static T GetInstance()
    {
        if (applicationIsQuitting)
        {
            return null;
        }

        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
            }
        }

        return instance;
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this as T)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
