using UnityEngine;
public class MonoBehaviourSingletonTemplate<T> : MonoBehaviour where T : MonoBehaviourSingletonTemplate<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T) this;
            // Debug.LogWarning(gameObject.name);
            // GameObject.DontDestroyOnLoad(this);
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }

    public static bool IsExistInstance
    {
        get
        {
            return instance != null;
        }
    }
}