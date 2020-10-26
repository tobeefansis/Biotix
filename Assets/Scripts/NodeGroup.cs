using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class NodeGroup : MonoBehaviour
{
    [SerializeField] Color groupColor;
    [SerializeField] [Range(0, 2)] float speed;
    [SerializeField] protected List<Node> nodes = new List<Node>();
    [SerializeField] UnityEvent OnLastGroup;
    [SerializeField] UnityEvent OnEmpty;


    public List<Node> Nodes { get => nodes; set => nodes = value; }
    public float Speed { get => speed; set => speed = value; }
    public Color GroupColor { get => groupColor; set => groupColor = value; }

    public void AddNode(Node node)
    {
        if (!Nodes.Contains(node)) Nodes.Add(node);
        if (LevelEditor.Instance.IsAddeting) return;
        var NotMineNodes = GameManager.Instance.AllNodes
            .Where(n => n.Group != this && n.Group != null)
            .ToList();
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
            Destroy(this.gameObject);
        }
    }
}
