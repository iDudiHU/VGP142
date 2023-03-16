using UnityEngine;

/// <summary>
/// Based on the Unity Community Singleton code sample, but adjusted to work
/// with MonoBehaviors.
/// Inherit from this with the pattern NewClass : Singelton<NewClass>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singelton<T> : MonoBehaviour where T : Component
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
                instance = (T)FindObjectOfType(typeof(T));

            if (!instance)
            {
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
