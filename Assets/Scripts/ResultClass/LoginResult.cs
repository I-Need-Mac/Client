using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LoginResult
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public LoginData data { get; set; }

    public class LoginData
    {
        public int id { get; set; }
        public string steam_id { get; set; }
        public string name { get; set; }
        public int admin_level { get; set; }
        public string login_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public bool is_use { get; set; }
    }
}
