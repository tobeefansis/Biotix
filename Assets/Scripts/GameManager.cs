using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singletone<GameManager>
{

    public UnityEvent OnWin;
    public UnityEvent OnLose;
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    public NodeGroup player;
    public List<Node> AllNodes;

    public void Play() => StartCoroutine(GetAllPause(true));

    public void Resume() => StartCoroutine(GetAllPause(false));

    IEnumerator GetAllPause(bool t)
    {
        GameObject.FindObjectsOfType<Transform>()
            .Select(n => n.GetComponent<IPause>())
            .Where(n => n != null)
            .ToList()
            .ForEach(n => n.IsPause = t);
        return null;
    }

    public void Win()
    {
        OnWin.Invoke();
    }

    public void WinThisGroup(NodeGroup nodeGroup)
    {
        if (nodeGroup == player)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    public void Lose()
    {
        OnLose.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        AllNodes = FindObjectsOfType<Node>().ToList();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
