using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class Swipe : Singletone<Swipe>, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField] List<Node> nodes = new List<Node>();
    [SerializeField] Transform cursor;
    [SerializeField] NodeGroup group;
    [SerializeField] bool isSwiper;
    [SerializeField] Branch branchPrefub;

    public UnityEvent OnComplite;
    public UnityEvent OnCreateBranch;
    public Transform canvas;
    public Node SelectNode;

    public List<Node> Nodes { get => nodes; set => nodes = value; }
    public NodeGroup Group { get => group; set => group = value; }
    public bool IsSwiper { get => isSwiper; set => isSwiper = value; }
    public Vector3 CursorPosition => cursor.position;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddNode(Node node)
    {
        if (nodes.Contains(node)) return false;
        nodes.Add(node);
        return true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        cursor.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSwiper = true;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Complite();
    }

    public void Complite()
    {
        if (SelectNode)
        {
            OnCreateBranch.Invoke();
            CreateBranchs();
        }
        Nodes.Clear();
        OnComplite.Invoke();
        isSwiper = false;
        OnComplite.RemoveAllListeners();
    }

    private void CreateBranchs()
    {
        foreach (var item in nodes)
        {
            if (item.Count == 0) continue;
            if (item == SelectNode) continue;
            var t = Instantiate(branchPrefub, canvas);
            var value = item.Count / 2;
            item.Count -= value;


            t.Set(item.transform, Group, SelectNode, value);
        }
    }
}
