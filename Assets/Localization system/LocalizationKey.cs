using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationKey : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] string key;
    void Start()
    {
        LocalizationSystem.Instate.OnReloadLocalization.AddListener(Reload);
        Reload();
    }

    private void Reload()
    {
        text.text = LocalizationSystem.Instate.GetText(key);
    }
}
