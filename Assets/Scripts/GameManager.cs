using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singletone<GameManager>
{

    [SerializeField] UnityEvent OnWin;
    [SerializeField] UnityEvent OnLose;
    [SerializeField] UnityEvent OnPause;
    [SerializeField] UnityEvent OnResume;
    [SerializeField] NodeGroup player;
    [SerializeField] List<Node> _AllNodes;

    public List<Node> AllNodes { get => _AllNodes; set => _AllNodes = value; }

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
        print("///////WIN///////");
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
        print("///////LOSE///////");
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
