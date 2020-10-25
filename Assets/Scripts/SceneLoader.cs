using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IJunior.TypedScenes;
using System;

public class SceneLoader : MonoBehaviour
{
    [Serializable]
    public enum Scenes
    {
        MainMenu,
        Settings,
        Info,
        Map,
        Win,
        Lose
    }
    public Scenes scene;
    public void LoadScene()
    {
        switch (scene)
        {
            case Scenes.MainMenu:
                MainMenu.Load();
                break;
            case Scenes.Settings:
                Settings.Load();
                break;
            case Scenes.Info:
                Info.Load();
                break;
            case Scenes.Map:
                Map.Load();
                break;
            default:
                break;
        }
    }
}
