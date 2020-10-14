using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : NodeGroup
{
    public enum Strategy
    {
        Peaceful,
        Nearest,
        Weak
    }
    public float time;
    public Strategy strategy;
    [SerializeField] Branch branchPrefub;
    public Transform canvas;
    // Use this for initialization
    void Start()
    {

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (Nodes.Count > 0)
        {
            yield return new WaitForSecondsRealtime(time);
            List<Node> enemys = GameManager.Instance.AllNodes
                .Where(n => n.Group != this)
                .ToList();

            if (enemys.Count == 0) break;
            Nodes = Nodes.OrderBy(n => n.Count).Reverse().ToList();
            var max = Nodes.First();

            Node target = null;
            switch (strategy)
            {
                case Strategy.Peaceful:
                    break;
                case Strategy.Nearest:
                    target = enemys.Min();
                    break;
                case Strategy.Weak:
                    target = enemys.Select(
                         n => new
                         {
                             target = n,
                             distance = Vector2.Distance(n.transform.position, max.transform.position)
                         }
                         ).Aggregate((a, b) => a.distance < b.distance ? a : b).target;

                    break;
                default:
                    break;
            }

            if (target)
            {
                List<Node> SelectedNodes = new List<Node>();
                foreach (var item in Nodes)
                {
                    SelectedNodes.Add(item);
                    if (SelectedNodes.Sum(n => n.Count) > target.Count)
                    {
                        break;
                    }
                }

                foreach (var item in SelectedNodes)
                {
                    var t = Instantiate(branchPrefub, canvas);
                    var value = item.Count / 2;
                    item.Count -= value;


                    t.Set(item.transform, this, target, value);
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}