using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager 
{
    private bool printDebug = true;
    private static DebugManager _instance { get; set; }
    public static DebugManager Instance
    {
        get
        {
            return _instance ?? (_instance = new DebugManager());
        }
    }

    public void PrintDebug(object target) {
        if (printDebug)
            Debug.Log(target);
        
    }
    public void PrintDebug(object target, object value)
    {
        if (printDebug)
            Debug.Log(target+" : "+value);

    }
    public void PrintDebug(string target)
    {
        if (printDebug)
            Debug.Log(target);



    }

    public void PrintDrawLine()
    {
        if (printDebug)
            Debug.Log("------------------------------------------------------------------");
    }

}
