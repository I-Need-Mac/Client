using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulProgress
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public SoulProgressData data { get; set; }

    public class SoulProgressData
    {
        public int id { get; set; }
        public string steam_id { get; set; }
        public int souls_id { get; set; }
        public int soul1_count { get; set; }
        public int soul2_count { get; set; }
        public int soul3_count { get; set; }
        public int soul4_count { get; set; }
        public int soul5_count { get; set; }
        public int soul6_count { get; set; }
        public int soul7_count { get; set; }
        public int soul8_count { get; set; }
        public int soul9_count { get; set; }
        public int soul10_count { get; set; }
        public int soul11_count { get; set; }
        public int soul12_count { get; set; }
        public int soul13_count { get; set; }
        public int soul14_count { get; set; }
        public int soul15_count { get; set; }
        public int soul16_count { get; set; }
        public int soul17_count { get; set; }
        public int soul18_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
