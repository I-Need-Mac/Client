using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicatedNickName
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public DuplicatedNickNameData data { get; set; }


    public class DuplicatedNickNameData
    {
        public bool isDuplicated { get; set; }
    }

}
