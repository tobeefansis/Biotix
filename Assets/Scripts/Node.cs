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
    [SerializeField] int MaxCount;
    [SerializeField] NodeGroup group;
    [SerializeField] Text countText;
    [SerializeField] Image image;
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
            ChangeCount();
        }
    }
    public NodeGroup Group
    {
        get => group;
        private set
        {
            if (group != null)
            {

                group.RemoveNode(this);
            }
            group = value;

            if (group == null)
            {
                OnStColor.Invoke(Color.white);
                Diselect();
            }
            else
            {
                group.AddNode(this);
                OnStColor.Invoke(group.GroupColor);
            }
        }
    }

    public void ChangeCount()
    {
        countText.text = Count.ToString();
    }

    IEnumerator Increment()
    {
        while (true)
        {
            if (group)
            {
                if (Count > MaxCount)
                {
                    Count--;
                }
                if (Count < MaxCount)
                {
                    Count++;
                }
                if (true)
                {

                }
                yield return new WaitForSeconds(1f / group.Speed);
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
        ChangeCount();
        if (group == null)
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
            print($"{group} {Group}");
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
        SelectWhitOutLine();
    }
    public void SelectWhitOutLine()
    {
        isSelect = true;
        OnSelect.Invoke();
    }

    public void Diselect()
    {

        isSelect = false;
        lineRenderer.enabled = false;
        OnDiselect.Invoke();
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
