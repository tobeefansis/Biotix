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
    public List<Level> levels = new List<Level>();

    bool IsAcceptLoad;
   [SerializeField] Level _selectLevel;
    public Level selectLevel { get => _selectLevel; set => _selectLevel = value; }
    public int selectLevelIndex => levels.IndexOf(selectLevel);

    public bool isLast
    {
        get => selectLevelIndex + 1 < levels.Count && levels[selectLevelIndex + 1].data == null;
    }
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
        IsAcceptLoad = true;
        GetUserData(obj.PlayFabId);
    }


    private void OnDestroy()
    {
        if (IsAcceptLoad)
        {
            Save();
        }
    }
    public void Save()
    {
        var levelDatas = levels
            .Where(n => n.data != null)
            .Select(n => n.data)
            .ToArray();
        TimeInGame += Time.time;

        SetUserData(
            new Dictionary<string, string>()
            {
                {Constants.UserName, UserName },
                {Constants.TimeInGame, TimeInGame.ToString() },
                {Constants.Levels, JsonHelper.ToJson(levelDatas)}
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
        if (data.ContainsKey(Constants.Levels))
        {
            var levelDatas = JsonHelper.FromJson<LevelData>(data[Constants.Levels].Value).ToList();
            if (levelDatas.Count == 0)
            {
                levelDatas.Add(new LevelData());
            }
            for (int i = 0; i < levels.Count; i++)
            {
                if (i < levelDatas.Count)
                {
                    levels[i].data = levelDatas[i];
                    levels[i].data.IsOpen = true;
                }
                else
                {
                    levels[i].data = null;
                }
            }

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
