using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnBoxResult
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public RewardBoxData data { get; set; }
}

public class RewardBoxData
{
    public List<OwnBoxData> userRewardBoxes { get; set; }
    public DateTime current_time { get; set; }
}

public class OwnBoxData
{
    public int id { get; set; }
    public string steam_id { get; set; }
    public int box_type { get; set; }
    public int stage_id { get; set; }
    public DateTime? open_start_time { get; set; }
    public bool is_open { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}
