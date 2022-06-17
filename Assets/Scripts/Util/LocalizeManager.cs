using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeManager : SingleTon<LocalizeManager>
{
    private string[] LANGUAGE = new string[] { "Korean", "English", "Japanese" };
    private int langType;
    private Dictionary<string, Dictionary<string, object>> localTableData = new Dictionary<string, Dictionary<string, object>>();

    public LocalizeManager() {
        SetLangType();
        GetLocalizeTable();        
    }

    public void SetLangType() {
        langType = SettingManager.Instance.GetSettingValue("lang");
    }

    public void GetLocalizeTable() { 
        localTableData = CSVReader.IDRead("Localize");
    }

    public string GetText(string targetID) {
        return Convert.ToString(localTableData[targetID][LANGUAGE[langType]]);
    }
}
