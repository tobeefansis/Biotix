using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [SerializeField] Transform from;
    [SerializeField] NodeGroup group;
    [SerializeField] ParticleSystem visual;
    [SerializeField] Node to;
    [SerializeField] int count;
    [SerializeField] [Range(0, 5)] float speed;

    [SerializeField] float time;

    public Transform From { get => from; set => from = value; }
    public Node To { get => to; set => to = value; }
    public int Count { get => count; set => count = value; }

    // Start is called before the first frame update
    void Start()
    {

        
        var posFrom = FromScreenToWorld(from.position);
        var posTo = FromScreenToWorld(to.transform.position);
        time = Vector2.Distance(posFrom, posTo) / speed;
        StartCoroutine(Translete(time));
        visual.transform.position = posFrom;
        visual.maxParticles = count;
        visual.transform.DOMove(posTo, time);
        
    }

    public void Set(Transform from, NodeGroup group, Node to, int count)
    {
        this.from = from;
        this.group = group;
        this.to = to;
        this.count = count;
    }

    IEnumerator Translete(float time)
    {
        yield return new WaitForSeconds(time);
        print($"{from.name} to {to.name}");
        To.Add(Count, group);
        Destroy(gameObject,3);
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
