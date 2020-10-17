using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class NodeGroup : JsonObject
{
    [SerializeField] Color groupColor;
    [SerializeField] [Range(0, 2)] float speed;
    [SerializeField] protected List<Node> Nodes = new List<Node>();
    [SerializeField] UnityEvent OnLastGroup;
    [SerializeField] UnityEvent OnEmpty;

    public Color GroupColor => groupColor;
    public float Speed => speed;

    public void AddNode(Node node)
    {
        Nodes.Add(node);
        var NotMineNodes = GameManager.Instance.AllNodes.Where(n => n.Group != this).ToList();
        if (NotMineNodes.Count == 0)
        {
            GameManager.Instance.WinThisGroup(this);
        }
    }

    public void RemoveNode(Node node)
    {
        Nodes.Remove(node);
        if (Nodes.Count == 0)
        {
            OnEmpty.Invoke();
        }
    }
}
