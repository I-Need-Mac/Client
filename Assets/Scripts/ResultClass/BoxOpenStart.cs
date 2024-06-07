using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuySorcererResult;

public class BoxOpenStart
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public BoxOpenStartData data { get; set; }

    public class BoxOpenStartData
    {
        public string steam_id { get; set; }
        public int id { get; set; }
        public DateTime? open_start_time { get; set; }
    }
}
