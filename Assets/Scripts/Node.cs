using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IComparable<Node>, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] int count;
    [SerializeField] int maxCount;
    [SerializeField] NodeGroup group;
    [SerializeField] Text countText;
    [SerializeField] UnityEvent<Color> OnStColor;
    [SerializeField] UnityEvent OnSelect;
    [SerializeField] UnityEvent OnDiselect;
    [SerializeField] LineRenderer lineRenderer;

    public bool isSelect;

    public int Count
    {
        get => count;
        set
        {
            count = value;
            countText.text = Count.ToString();
        }
    }

    public NodeGroup Group
    {
        get => group;
        set
        {
            if (value != group)
            {
                group?.RemoveNode(this);
                group = value;
                if (group == null)
                {
                    OnStColor.Invoke(Color.white);
                }
                else
                {
                    group.AddNode(this);
                    OnStColor.Invoke(group.GroupColor);
                    Diselect();
                }
            }
        }
    }

    public int MaxCount { get => maxCount; set => maxCount = value; }


    IEnumerator Increment()
    {
        while (true)
        {
            if (Group)
            {
                if (Count > MaxCount)
                {
                    Count--;
                }
                if (Count < MaxCount)
                {
                    Count++;
                }
                yield return new WaitForSeconds(1f / Group.Speed);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void Start()
    {
        StartCoroutine(Increment());

        if (Group == null)
        {
            OnStColor.Invoke(Color.white);
        }
        else
        {
            group.AddNode(this);
            OnStColor.Invoke(group.GroupColor);

        }
    }

    public Vector3 FromScreenToWorld(Vector3 pos)
    {
        var t = Camera.main.ScreenToWorldPoint(pos);
        t.z = 0;
        return t;
    }

    private void LateUpdate()
    {
        if (isSelect)
        {
            var posFrom = FromScreenToWorld(transform.position);
            var posTo = FromScreenToWorld(Swipe.Instance.CursorPosition);
            lineRenderer.SetPosition(0, posFrom);
            lineRenderer.SetPosition(1, posTo);
        }
    }

    public void Add(int count, NodeGroup group)
    {
        if (Group == group)
        {
            Count += count;
        }
        else
        {
            Count -= count;
            if (Count < 0)
            {
                Group = group;
                Count = Count * -1;
            }
            if (Count == 0)
            {
                Group = null;
            }
        }

    }

    public void Select()
    {
        lineRenderer.enabled = true;
        OnSelect.Invoke();
        SelectWhitOutLine();
    }

    public void SelectWhitOutLine()
    {
        isSelect = true;
        OnSelect.Invoke();
    }

    public void Diselect()
    {
        print(name);
        isSelect = false;
        lineRenderer.enabled = false;
        OnDiselect.Invoke();
    }

    public void Send()
    {
        print(name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Swipe.Instance.IsSwiper)
        {
            if (Swipe.Instance.Group == Group)
            {
                if (Swipe.Instance.AddNode(this))
                {
                    Select();
                    Swipe.Instance.OnComplite.AddListener(Diselect);
                }
            }
            Swipe.Instance.SelectNode = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Swipe.Instance.SelectNode == this)
        {
            Swipe.Instance.SelectNode = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Swipe.Instance.Group == Group)
        {
            if (Swipe.Instance.AddNode(this))
            {
                SelectWhitOutLine();
                Swipe.Instance.OnComplite.AddListener(Diselect);
            }
            else
            {
                Swipe.Instance.SelectNode = this;
                Swipe.Instance.Complite();
            }
        }
        else
        {
            Swipe.Instance.SelectNode = this;
            Swipe.Instance.Complite();
        }
    }

    public int CompareTo(Node other)
    {
        return Count.CompareTo(other.Count);

    }
}
