using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
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
}