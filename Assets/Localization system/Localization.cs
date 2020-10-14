using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[CreateAssetMenu()]
public class Localization : ScriptableObject
{
    [System.Serializable]
    public struct Translation
    {
        public string Key;
        public string Text;
    }

    [SerializeField] List<Translation> translations = new List<Translation>();

    public List<Translation> Translations { get => translations; set => translations = value; }
}
