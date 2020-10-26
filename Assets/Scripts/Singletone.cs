using UnityEngine;
using System.Collections;

public class Singletone<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
#if !UNITI_EDITOR
            Destroy(this);

#endif
        }
    }
}
