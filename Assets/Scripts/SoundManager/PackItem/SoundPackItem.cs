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
    public AudioClip[] audioClipList_ENG;
    public AudioClip[] audioClipList_JP;
    public AudioClip targetClip
    {
        get
        {
            if (LocalizeManager.Instance.GetLangType() == 0)
            {
                return korSounds;
            }
            else if (LocalizeManager.Instance.GetLangType() == 1)
            {
                if (audioClipList_ENG != null)
                {
                    if (audioClipList_ENG.Length != 0)
                    {
                        int rnd = Random.Range(0, audioClipList_ENG.Length);
                        if (audioClipList_ENG[rnd] != null)
                            return audioClipList_ENG[rnd];
                        else return korSounds;
                    }
                    else return korSounds;
                }
                else return korSounds;
            }
            else if (LocalizeManager.Instance.GetLangType() == 2)
            {
                if (audioClipList_JP != null)
                {
                    if (audioClipList_JP.Length != 0)
                    {
                        int rnd = Random.Range(0, audioClipList_JP.Length);
                        if (audioClipList_JP[rnd] != null)
                            return audioClipList_JP[rnd];
                        else return korSounds;
                    }
                    else return korSounds;
                }
                else return korSounds;
            }
            return korSounds;
        }
    }
    public AudioClip korSounds
    {
        get
        {
            if (audioClipList != null)
            {
                if (audioClipList.Length != 0)
                {
                    int rnd = Random.Range(0, audioClipList.Length);
                    if (audioClipList[rnd] != null)
                        return audioClipList[rnd];
                    else return audioClip;
                }
                else return audioClip;
            }
            else return audioClip;
        }
    }

}
