using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class NodeGroup : MonoBehaviour
{
    [SerializeField] Color groupColor;
    [SerializeField] [Range(0, 2)] float speed;
    [SerializeField] protected List<Node> Nodes = new List<Node>();
    [SerializeField] UnityEvent OnLastGroup;
    [SerializeField] UnityEvent OnEmpty;

    public Color GroupColor { get => groupColor; set => groupColor = value; }
    public float Speed { get => speed; set => speed = value; }

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
