using UnityEngine;
using System.Collections;

public class Singletone<T> : MonoBehaviour where T : Component
{
    public static T Instate { get; private set; }

    private void Awake()
    {
        if (Instate == null)
        {
            Instate = this as T;
        }
        else
        {
            Destroy(this);
        }
    }
}