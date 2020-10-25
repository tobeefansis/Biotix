using System.Collections;
using System.Collections.Generic;
using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

public class WinController : MonoBehaviour, ISceneLoadHandler<GameManager.GameResult>
{
    [SerializeField] GameObject WinLabel;
    [SerializeField] GameObject LoseLabel;
    [Space]
    [SerializeField] LocalizationKey Tips;
    [SerializeField] List<string> WinTips = new List<string>();
    [SerializeField] List<string> LoseTips = new List<string>();
    [Space]
    [SerializeField] Text Score;
    [SerializeField] Text TimeInGame;

    public void OnSceneLoaded(GameManager.GameResult argument)
    {
        WinLabel.SetActive(argument.IsWin);
        LoseLabel.SetActive(!argument.IsWin);

        if (argument.IsWin)
        {
            Tips.Key = WinTips[Random.Range(0, WinTips.Count)];
        }
        else
        {
            Tips.Key = LoseTips[Random.Range(0, LoseTips.Count)];
        }

        Score.text = argument.Score.ToString();
        TimeInGame.text = argument.GameTime.ToString();

    }
}
