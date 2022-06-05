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

    public void PrintDebug(Object target) {
        if (printDebug)
            Debug.Log(target);
        
    }
    public void PrintDebug(Object target, Object value)
    {
        if (printDebug)
            Debug.Log(target+" : "+value);

    }





}
