using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public BoxOpenData data { get; set; }

    public class BoxOpenData
    {
        public int id { get; set; }
        public string steam_id { get; set; }
    }
}
