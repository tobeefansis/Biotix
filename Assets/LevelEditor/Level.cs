using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Level : ScriptableObject
{
    public List<string> nodes = new List<string>();
    public List<string> enemys = new List<string>();
    public string player;
}