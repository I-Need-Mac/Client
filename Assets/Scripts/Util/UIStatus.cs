using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatus : SingleTon<UIStatus>
{
    enum Sorcerers
    {
        hojin = 101,
        sinwol = 102,
        siWoo = 103,
        seimei = 104,
        macia = 105,
        ulises = 106
    }



    public int sorcererCost {get=> Convert.ToInt32(SettingManager.Instance.GetConfigSetting("Sorcerer"));}
    public string steam_id { get; set; }
    public string nickname { get; set; }
    public int selectedChar { get;set; }
    public string selectedChar_string { get; set; }

    public int? high_stage { get; set; }
    public int? last_stage { get; set; }
    public bool last_is_finished { get; set; }
    public bool last_is_clear { get; set; }
    public int? last_saint_soul_type { get; set; }
    public int? last_soul1 { get; set; }
    public int? last_soul2 { get; set; }
    public int? last_soul3 { get; set; }
    public int? last_soul4 { get; set; }
    public int? last_soul5 { get; set; }
    public int? last_soul6 { get; set; }
    public string last_character { get; set; }
    public int key { get; set; }
    public bool hojin { get; set; }
    public bool seimei { get; set; }
    public bool macia { get; set; }
    public bool sinwol { get; set; }
    public bool siWoo { get; set; }
    public bool ulises { get; set; }

    private int GetSorcerer() {
        if (selectedChar_string.Equals(Sorcerers.hojin.ToString())) { 
            return (int)Sorcerers.hojin;
        }
        else if (selectedChar_string.Equals(Sorcerers.sinwol.ToString())) {
            return (int)Sorcerers.sinwol;
        }
        else if (selectedChar_string.Equals(Sorcerers.siWoo.ToString()))
        {
            return (int)Sorcerers.siWoo;
        }
        else if (selectedChar_string.Equals(Sorcerers.seimei.ToString()))
        {
            return (int)Sorcerers.seimei;
        }
        else if (selectedChar_string.Equals(Sorcerers.macia.ToString()))
        {
            return (int)Sorcerers.macia;
        }
        else if (selectedChar_string.Equals(Sorcerers.ulises.ToString()))
        {
            return (int)Sorcerers.ulises;
        }
        else { 
            return 0;
        }

    }
}
