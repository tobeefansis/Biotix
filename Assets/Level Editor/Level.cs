using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu()]
[System.Serializable]
public class Level : ScriptableObject
{
    [SerializeField] int id;
    public int ID => id;
    [SerializeField] List<string> emptyNodes = new List<string>();
    [SerializeField] List<string> enemys = new List<string>();
    [SerializeField] string player;

    public NodeGroupDate GetPlayer()
    {
        return JsonUtility.FromJson<NodeGroupDate>(player);
    }

    public void SetPlayer(NodeGroup value)
    {
        player = JsonUtility.ToJson(new NodeGroupDate(value));
    }

    public List<EnemyDate> GetEnemies() => enemys
        .Select(n => JsonUtility.FromJson<EnemyDate>(n))
        .ToList();
    public void SetEnemys(List<Enemy> value) => enemys = value
        .Select(n => JsonUtility.ToJson(new EnemyDate(n)))
        .ToList();

    public List<NodeDate> GetEmptyNodes() => emptyNodes
        .Select(n => JsonUtility.FromJson<NodeDate>(n))
        .ToList();

    public void SetEmptyNodes(List<Node> value) => emptyNodes = value
        .Select(n => JsonUtility.ToJson(new NodeDate(n)))
        .ToList();

    [System.Serializable]
    public class EnemyDate : NodeGroupDate
    {
        public Enemy.Strategy strategy;

        public EnemyDate(Enemy enemy) : base(enemy)
        {
            strategy = enemy.GetStrategy();
            position = enemy.transform.position;
        }

    }

    [System.Serializable]
    public class NodeGroupDate
    {
        public Vector3 position;
        public Color groupColor;
        public float speed;
        public List<NodeDate> Nodes = new List<NodeDate>();

        public NodeGroupDate(NodeGroup value)
        {
            position = value.transform.position;
            groupColor = value.GroupColor;
            speed = value.Speed;
            Nodes = value.Nodes.Select(n => new NodeDate(n)).ToList();

        }
    }

    [System.Serializable]
    public struct NodeDate
    {
        public Vector3 position;
        public int Count;
        public int MaxCount;

        public NodeDate(Node node)
        {
            Count = node.Count;
            MaxCount = node.MaxCount;
            position = node.transform.position;
        }
    }

}