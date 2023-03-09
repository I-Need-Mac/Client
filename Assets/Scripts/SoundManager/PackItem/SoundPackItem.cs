using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundPackItem : PackItem
{
    [SerializeField] public SoundSituation.SOUNDSITUATION SOUNDSITUATION;

    [SerializeField] public AudioClip audioClip;
}
