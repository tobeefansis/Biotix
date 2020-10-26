using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.Events;
using IJunior.TypedScenes;

public class GameManager : Singletone<GameManager>, IUnityAdsListener
{

    [SerializeField] UnityEvent OnWin;
    [SerializeField] UnityEvent OnLose;
    [SerializeField] UnityEvent OnPause;
    [SerializeField] UnityEvent OnResume;
    [SerializeField] NodeGroup player;
    [SerializeField] List<Node> _AllNodes;

    public List<Node> AllNodes { get => _AllNodes; set => _AllNodes = value; }
    public NodeGroup Player { get => player; set => player = value; }

    public void Play() => StartCoroutine(GetAllPause(true));

    public void Resume() => StartCoroutine(GetAllPause(false));

    bool IsWin;

    float time;

    IEnumerator GetAllPause(bool t)
    {
        FindObjectsOfType<Transform>()
            .Select(n => n.GetComponent<IPause>())
            .Where(n => n != null)
            .ToList()
            .ForEach(n => n.IsPause = t);
        return null;
    }

    public void SetWin()
    {
        print("///////WIN///////");
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
            Advertisement.AddListener(this);
        }
        IsWin = true;
        var settings = PlayerSettings.Instance;
        if (settings.isLast)
        {
            var next = settings.levels[settings.selectLevelIndex + 1];
            next.data = new LevelData() { IsOpen = true};
            Debug.LogError("ADD lEVEL");
        }
        var t = Time.time - time;
        if (settings.selectLevel.data.MinTime > t)
        {
            settings.selectLevel.data.MinTime = t;
        }
        settings.Save();
        OnWin.Invoke();
    }

    public void WinThisGroup(NodeGroup nodeGroup)
    {
        if (nodeGroup == Player)
        {
            SetWin();
        }
        else
        {
            SetLose();
        }
    }

    public void SetLose()
    {
        print("///////LOSE///////");
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
        }
        IsWin = false;
        OnLose.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        AllNodes = FindObjectsOfType<Node>().ToList();
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("3874383", false);
        }
        time = Time.time;
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:

                break;
            case ShowResult.Skipped:

                break;
            case ShowResult.Finished:

                break;
            default:
                break;
        }
        Win.Load(new GameResult() { IsWin = IsWin, GameTime = Mathf.RoundToInt(Time.time - time) });
    }

    public struct GameResult
    {
        public float GameTime;
        public float Score;
        public bool IsWin;
    }
}
