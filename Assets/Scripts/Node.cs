using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] int count;
    [SerializeField] NodeGroup group;
    [SerializeField] Text countText;
    [SerializeField] Image image;
    [SerializeField] UnityEvent OnSelect;
    [SerializeField] UnityEvent OnDiselect;
    [SerializeField] LineRenderer lineRenderer;

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
        set
        {
            group = value;
            if (group == null)
            {
                image.color = Color.white;
            }
            else
            {
                image.color = group.GroupColor;
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
                Count++;
                yield return new WaitForSeconds(1f / group.Speed);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public bool isSelect;

    void Start()
    {
        StartCoroutine(Increment());
        ChangeCount();
        if (group == null)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = group.GroupColor;
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
            var posTo = FromScreenToWorld(Swipe.Instate.CursorPosition);
            lineRenderer.SetPosition(0, posFrom);
            lineRenderer.SetPosition(1, posTo);
        }
    }

    public void Add(int count, NodeGroup group)
    {
        var t = Count;
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
        print($"{gameObject.name } change from{t} to {Count}  --- {count - Count}");
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
        if (Swipe.Instate.IsSwiper)
        {
            if (Swipe.Instate.Group == Group)
            {
                if (Swipe.Instate.AddNode(this))
                {
                    Select();
                    Swipe.Instate.OnComplite.AddListener(Diselect);
                }
            }
            Swipe.Instate.SelectNode = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Swipe.Instate.SelectNode == this)
        {
            Swipe.Instate.SelectNode = null;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Swipe.Instate.Group == Group)
        {
            if (Swipe.Instate.AddNode(this))
            {
                SelectWhitOutLine();
                Swipe.Instate.OnComplite.AddListener(Diselect);
            }
            else
            {
                Swipe.Instate.SelectNode = this;
                Swipe.Instate.Complite();
            }
        }
        else
        {
            Swipe.Instate.SelectNode = this;
            Swipe.Instate.Complite();
        }
    }
}
