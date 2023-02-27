using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundPackItem
{
    [SerializeField] public SoundSituation.SOUNDSITUATION SOUNDSITUATION;
    [SerializeField] public string usingSpeaker = "Defualt";
    [SerializeField] [Range (0,60)]public float delay = 0f;
    [SerializeField] public AudioClip audioClip;
}
