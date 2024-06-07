using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIStatus : SingleTon<UIStatus>
{
    public enum Sorcerers
    {
        hojin = 101,
        sinwol = 102,
        siWoo = 103,
        seimei = 104,
        ulises = 105,
        macia = 106

    }



    public int sorcererCost { get => Convert.ToInt32(SettingManager.Instance.GetConfigSetting("Sorcerer")); }
    public string steam_id { get; set; }
    public string nickname { get; set; }
    public int selectedChar { get; set; }
    public string selectedChar_string { get; set; }

    public UI_SelectSorcerer uI_SelectSorcerer { get; set; }

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
    public List<UI_Jusulso_Box> boxList { get; set; }
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
    public  int GetSorcerer(string name)
    {
        if (name.Equals(Sorcerers.hojin.ToString()))
        {
            return (int)Sorcerers.hojin;
        }
        else if (name.Equals(Sorcerers.sinwol.ToString()))
        {
            return (int)Sorcerers.sinwol;
        }
        else if (name.Equals(Sorcerers.siWoo.ToString()))
        {
            return (int)Sorcerers.siWoo;
        }
        else if (name.Equals(Sorcerers.seimei.ToString()))
        {
            return (int)Sorcerers.seimei;
        }
        else if (name.Equals(Sorcerers.macia.ToString()))
        {
            return (int)Sorcerers.macia;
        }
        else if (name.Equals(Sorcerers.ulises.ToString()))
        {
            return (int)Sorcerers.ulises;
        }
        else
        {
            return 0;
        }

    }
    public void SetSorcerer(int sorcererID, bool locked)
    {
        switch (sorcererID)
        {
            case (int)Sorcerers.hojin:
                hojin= locked;
                break;
            case (int)Sorcerers.sinwol:
                sinwol= locked;
                break;
            case (int)Sorcerers.siWoo:
                siWoo= locked;
                break;
            case (int)Sorcerers.seimei:
                seimei= locked;
                break;
            case (int)Sorcerers.ulises:
                ulises= locked;
                break;
            case (int)Sorcerers.macia:
                macia= locked;
                break;
            default:
                break;
        }
    }
    public string GetSorcerer(int sorcererID)
    {
        switch (sorcererID) {
            case (int)Sorcerers.hojin:
                return Sorcerers.hojin.ToString();
            case (int)Sorcerers.sinwol:
                return Sorcerers.sinwol.ToString();
            case (int)Sorcerers.siWoo:
                return Sorcerers.siWoo.ToString();
            case (int)Sorcerers.seimei:
                return Sorcerers.seimei.ToString();
            case (int)Sorcerers.ulises:
                return Sorcerers.ulises.ToString();
            case (int)Sorcerers.macia:
                return Sorcerers.macia.ToString();
            default :
                return "Wrong ID";
        }

    }
}
