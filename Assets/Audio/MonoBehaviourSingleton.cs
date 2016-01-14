using UnityEngine;
using System.Collections;

/// <summary>
/// Generic Singleton extending Monobehaviour
/// 
/// On First Get finds object of its type or instantiates a new object of this type
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour 
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<T>();
                    go.name = "Singleton" + typeof(T).ToString();
                }
            }
            return _instance;
        }
    }
}
