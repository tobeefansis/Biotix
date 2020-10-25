using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LocalizationKey : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] string key;

    public string Key { get => key; set => key = value; }

    void Start()
    {
        LocalizationSystem.Instance.OnReloadLocalization.AddListener(Reload);
        Reload();
    }

    private void Reload()
    {
        text.text = LocalizationSystem.Instance.GetText(key);
    }
}
