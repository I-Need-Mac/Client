using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySorcererResult
{


        public int statusCode { get; set; }
        public string message { get; set; }
        public BuySorcererData data { get; set; }
 

    public class BuySorcererData
    {
        public string steam_id { get; set; }
        public int keys { get; set; }
        public string character { get; set; }
    }

}

