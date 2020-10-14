using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [SerializeField] Transform from;
    [SerializeField] NodeGroup group;
    [SerializeField] Transform visual;
    [SerializeField] Node to;
    [SerializeField] int count;
    [SerializeField] [Range(0, 5)] float speed;

    float time;

    public Transform From { get => from; set => from = value; }
    public Node To { get => to; set => to = value; }
    public int Count { get => count; set => count = value; }

    // Start is called before the first frame update
    void Start()
    {
        var posFrom = FromScreenToWorld(from.transform.position);
        var posTo = FromScreenToWorld(to.transform.position);
        time = Vector2.Distance(posFrom, posTo) / speed;
        StartCoroutine(Translete());
        visual.position = from.position;
        visual.DOMove(to.transform.position, time);
        
    }

    public void Set(Transform from, NodeGroup group, Node to, int count)
    {
        this.from = from;
        this.group = group;
        this.to = to;
        this.count = count;
    }

    IEnumerator Translete()
    {
        yield return new WaitForSeconds(time);

        To.Add(Count, group);
        Destroy(gameObject, 1);
    }
    public Vector3 FromScreenToWorld(Vector3 pos)
    {
        var t = Camera.main.ScreenToWorldPoint(pos);
        t.z = 0;
        return t;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
