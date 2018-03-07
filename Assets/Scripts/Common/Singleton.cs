using UnityEngine;
using System.Collections;

public partial class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            if (applicationQuit)
                return null;

            if (instance == null)
                instance = new GameObject(typeof(T).Name).AddComponent<T>();

            DontDestroyOnLoad(instance.gameObject);

            return instance;
        }
    }

    public static bool HasInstance { get { return instance != null; } }

    public static void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;

        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        applicationQuit = true;
    }
}

#region Implements

public partial class Singleton<T>
{
    static T instance;
    static bool applicationQuit;
}

#endregion
