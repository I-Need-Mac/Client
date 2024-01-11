using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundPackItem : PackItem
{
    [SerializeField] public SoundSituation.SOUNDSITUATION SOUNDSITUATION;

    [SerializeField]
    public AudioClip audioClip;
    [SerializeField]
    public AudioClip[] audioClipList;

    public AudioClip targetClip
    {
        get {
            if (audioClipList.Length != 0)
            {
                int rnd = Random.Range(0, audioClipList.Length);
                return audioClipList[rnd];
            }
            else return audioClip;
        }
    }
}
