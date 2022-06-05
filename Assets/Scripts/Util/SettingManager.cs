using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingManager
{
    private Dictionary<string, int> _settings { get; set; }
    public Dictionary<string, int> settings
    {
        set { 
            _settings = value;
        }

        get { 
            return _settings ??( _settings= new Dictionary<string, int>());
        }
    }

    private FileStream settingFile = new FileStream("./setting.txt", FileMode.Open);


    private static SettingManager _instance { get; set; }
    public static SettingManager Instance
    {
        get
        {

            return _instance ?? (_instance = new SettingManager());
        }
    }

    public void ReadSettingFile() {
        StreamReader sr = new StreamReader(settingFile);              


        string source = sr.ReadLine();       
        string [] values;
        while (source != null)
        {
            values = source.Split('=');  // 쉼표로 구분한다. 저장시에 쉼표로 구분하여 저장하였다.           
            if( values.Length == 0 ){               
                sr.Close();                
                return;            
            }           
            settings.Add(values[0],int.Parse(values[1]));
            source = sr.ReadLine();    // 한줄 읽는다.        
        }
        sr.Close();
    }

    public int GetSettingValue(String target) { 
        if(settings.TryGetValue(target, out int value)){
            return value;
        }
        return -1;
        
    }
        
}
