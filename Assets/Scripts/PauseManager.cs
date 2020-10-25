using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class PauseManager : Singletone<PauseManager>
{
    [SerializeField] List<IPause> PauseObjects = new List<IPause>();
    [SerializeField] UnityEvent OnPause;
    [SerializeField] UnityEvent OnResume;
    private void Start()
    {
        Time.timeScale = 1;
        PauseObjects = FindObjectsOfType<GameObject>()
            .Select(n => n.GetComponent<IPause>())
            .Where(n => n != null)
            .ToList();
    }

    public void Pause()
    {
        PauseObjects.ForEach(n => n.Pause());
        OnPause.Invoke();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PauseObjects.ForEach(n => n.Resume());
        OnResume.Invoke();
        Time.timeScale = 1;
    }
}