using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class LocalizationSystem : Singletone<LocalizationSystem>
{
    enum Erorrs
    {
        No_Localization_File,
        No_Key_In_List,
        No_Value_In_Translation
    }

    [SerializeField] Localization selectLocalization;
    public UnityEvent OnReloadLocalization;

    public void SetLocalization(Localization localization)
    {
        selectLocalization = localization;
        if (localization == null) Debug.LogError("No_Localization_File");
        OnReloadLocalization?.Invoke();
    }

    public string GetText(string key)
    {
        if (selectLocalization == null) return Erorrs.No_Localization_File.ToString();

        var t = selectLocalization.Translations
            .Where(n => n.Key == key)
            .ToList();

        if (t.Count == 0) return Erorrs.No_Key_In_List.ToString();

        if (t[0].Text == "") return Erorrs.No_Value_In_Translation.ToString();

        return t[0].Text;

    }
}
