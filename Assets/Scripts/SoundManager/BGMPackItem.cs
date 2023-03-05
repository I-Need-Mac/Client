using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BGMPackItem : PackItem
{
    [SerializeField] public BGMSituation.BGMSITUATION BGMSITUATION;

    [SerializeField] public AudioClip introBGMClip;
    [SerializeField] [Range(0, 60)] public float delayWithIntro = 0f;
    [SerializeField] public AudioClip realBGMClip;
}

