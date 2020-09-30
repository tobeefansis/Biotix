using UnityEngine;
using System.Collections;

public class NodeGroup: MonoBehaviour
{
    [SerializeField] Color groupColor;
    [SerializeField] [Range(0, 2)] float speed;

    public Color GroupColor { get => groupColor; set => groupColor = value; }
    public float Speed { get => speed; set => speed = value; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
