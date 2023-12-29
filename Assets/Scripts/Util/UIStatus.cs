using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatus : SingleTon<UIStatus>
{
    public int sorcererCost {get=> Convert.ToInt32(SettingManager.Instance.GetConfigSetting("Sorcerer"));}
    public string steam_id { get; set; }
    public string nickname { get; set; }
    public int selectedChar { get; set; }

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
}
