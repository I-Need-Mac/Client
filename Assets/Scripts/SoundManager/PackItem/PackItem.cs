using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackItem   
{
    [SerializeField] public string usingSpeaker = "Defualt";
    [SerializeField] [Range(0, 60)] public float delay = 0f;
    [SerializeField] [Range(0, 10)] public int priority = 5;
}
