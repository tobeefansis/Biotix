using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerSettings : Singletone<PlayerSettings>
{
    public string UserName;
    public float TimeInGame;
    public DateTime LastVisit;
    public List<LevelDate> levelDatas = new List<LevelDate>();
    public List<Level> levels = new List<Level>();
 

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "DE8D3";
        }
        var request = new LoginWithAndroidDeviceIDRequest() { AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, Accept, OnError);
    }

    private void Accept(LoginResult obj)
    {
        GetUserData(obj.PlayFabId);
    }

    public void Save()
    {
        string[] t = { "asd", "asd" };
        print(JsonUtility.ToJson(t));
        SetUserData(
            new Dictionary<string, string>()
            {
                {Constants.UserName, UserName },
                {Constants.TimeInGame, TimeInGame.ToString() },
                {Constants.LastVisit, LastVisit.Ticks.ToString() },
                {Constants.Levels, JsonHelper.ToJson(levels.ToArray())}
            }
            );
    }

    void GetUserData(string myPlayFabeId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, GetUserDateSuccess, OnError);
    }
    void SetUserData(Dictionary<string, string> data)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>(data)
        }, result => Debug.Log("Update User Date Success"), OnError);
    }

    public void LoadLevel()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
        LoadLeveSuccess,
        OnError
    );
    }

    private void LoadLeveSuccess(GetTitleDataResult result)
    {
        if (result == null) return;


    }

    private void GetUserDateSuccess(GetUserDataResult result)
    {
        if (result.Data == null) return;
        var data = result.Data;
        if (data.ContainsKey(Constants.UserName))
        {
            UserName = data[Constants.UserName].Value;
        }
        if (data.ContainsKey(Constants.TimeInGame))
        {
            TimeInGame = float.Parse(data[Constants.TimeInGame].Value);
        }
        if (data.ContainsKey(Constants.LastVisit))
        {
            LastVisit = new DateTime(long.Parse(data[Constants.LastVisit].Value));
        }
        if (data.ContainsKey(Constants.Levels))
        {
            levelDatas = JsonHelper.FromJson<LevelDate>(data[Constants.Levels].Value).ToList();
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }


    public struct Date
    {

    }
}
