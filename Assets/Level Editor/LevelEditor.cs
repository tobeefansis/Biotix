using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using IJunior.TypedScenes;
using UnityEditor;

[RequireComponent(typeof(Swipe))]
public class LevelEditor : MonoBehaviour, ISceneLoadHandler<Level>
{
    public Level level { get; set; }
    [SerializeField] Transform Nodes;
    [SerializeField] Transform Player;
    [SerializeField] Node NodePrefub;
    [SerializeField] NodeGroup NodeGroupPrefub;
    [SerializeField] Enemy EnemyPrefub;
    [SerializeField] Transform Enemies;


    public void Load(Level level)
    {
        this.level = level;
        Load();
    }
    public void Save()
    {
        List<Enemy> enemies = GetEnemies();

        List<Node> nodes = GetNodes();

        SavePlayer();

        level.SetEmptyNodes(nodes);
        level.SetEnemys(enemies);

        List<Enemy> GetEnemies()
        {
            List<Enemy> _enemies = new List<Enemy>();
            foreach (Transform item in Enemies)
            {
                var enemy = item.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.Nodes.Clear();
                    _enemies.Add(enemy);
                }
            }
            return _enemies;
        }
       // EditorUtility.SetDirty(level);
    }

    private void SavePlayer()
    {
        foreach (Transform item in Player)
        {
            var nodeGroup = item.GetComponent<NodeGroup>();
            if (nodeGroup)
            {
                level.SetPlayer(nodeGroup);
                break;
            }
        }
    }

    private List<Node> GetNodes()
    {
        List<Node> nodes = new List<Node>();
        foreach (Transform item in this.Nodes)
        {
            var node = item.GetComponent<Node>();
            if (node)
            {
                if (node.Group == null)
                {
                    nodes.Add(node);
                }
                else
                {
                    var groupe = node.Group;
                    if (!groupe.Nodes.Contains(node))
                    {
                        groupe.Nodes.Add(node);
                    }
                }
            }
        }

        return nodes;
    }

    public void Clear(Transform root)
    {
        while (root.childCount > 0)
        {
            DestroyImmediate(root.GetChild(0).gameObject);
        }
    }
    public void Clear()
    {

        Clear(Nodes);
        Clear(Player);
        Clear(Enemies);

    }

    public void Load()
    {
        Clear(Nodes);
        Clear(Player);
        Clear(Enemies);

        LoadPlayer();

        LoadEmptyNodes();

        LoadEnemies();
    }

    private void LoadEnemies()
    {
        var tempListEnemies = level.GetEnemies();

        foreach (var item in tempListEnemies)
        {
            var enemy = Instantiate(EnemyPrefub, item.position, Quaternion.identity, Enemies);
            enemy.Speed = item.speed;
            enemy.GroupColor = item.groupColor;
            enemy.SetStrategy(item.strategy);

            foreach (var nodedate in item.Nodes)
            {

                var node = Instantiate(NodePrefub, nodedate.position, Quaternion.identity, Nodes);
                node.MaxCount = 100;
                node.Count = nodedate.Count;
                node.GroupWithoutnNotification = enemy;
                enemy.Nodes.Add(node);
            }
        }
    }

    private void LoadEmptyNodes()
    {
        var tempListNodes = level.GetEmptyNodes();

        foreach (var item in tempListNodes)
        {
            var node = Instantiate(NodePrefub, item.position, Quaternion.identity, Nodes);
            node.MaxCount = 100;
            node.Count = item.Count;
        }
    }

    private void LoadPlayer()
    {
        var player = level.GetPlayer();
        var playerGroup = Instantiate(NodeGroupPrefub, player.position, Quaternion.identity, Player);
        playerGroup.Speed = player.speed;
        playerGroup.GroupColor = player.groupColor;
        FindObjectOfType<GameManager>().Player = playerGroup;
        GetComponent<Swipe>().Group = playerGroup;
        foreach (var nodedate in player.Nodes)
        {
            var node = Instantiate(NodePrefub, nodedate.position, Quaternion.identity, Nodes);
            node.MaxCount = nodedate.MaxCount;
            node.Count = nodedate.Count;
            node.GroupWithoutnNotification = playerGroup;
            playerGroup.Nodes.Add(node);
        }
    }

    public void OnSceneLoaded(Level argument)
    {
        Load(argument);
    }
}
