using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoicePackItem 
{
    // Start is called before the first frame update
   

    [SerializeField]
    public AudioClip[] audioClipList_KOR;
    public AudioClip[] audioClipList_ENG;
    public AudioClip[] audioClipList_JP;


    public AudioClip[] GetSoundList() {
        switch (LocalizeManager.Instance.GetLangType()) {
            case 0:
               return audioClipList_KOR;
            case 1:
                if (audioClipList_ENG.Length == 0) return audioClipList_KOR;
                else return audioClipList_ENG;

            case 2:
                if (audioClipList_JP.Length == 0) return audioClipList_KOR;
                else return audioClipList_JP;
            default :
                return audioClipList_KOR;
        }
    }



}
