using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BoxOpen
//{
//    public int statusCode { get; set; }
//    public string message { get; set; }
//    public BoxOpenData data { get; set; }

//    public class BoxOpenData
//    {
//        public int id { get; set; }
//        public string steam_id { get; set; }
//    }
//}
public class BoxOpen
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public BoxOpenData data { get; set; }
}
public class BoxOpenData
{
    public string steam_id { get; set; }
    public int id { get; set; }
    public int reward_box_id { get; set; }
    public Rewardbox reward1 { get; set; }
    public Rewardbox reward2 { get; set; }
    public Rewardbox reward3 { get; set; }
    public Rewardbox reward4 { get; set; }
    public DateTime created_at { get; set; }
}
public class Rewardbox
{
    public int id { get; set; }
    public string item { get; set; }
    public int amount { get; set; }
    public int weight { get; set; }
}