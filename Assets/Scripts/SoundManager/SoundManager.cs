using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AudioSourceSetter;

public class SoundManager : SingleTon<SoundManager>
{
 
    [SerializeField]
    Dictionary<string, AudioSource> audioSourceList = null;
   
    

    GameObject soundManager;
    GameObject soundRequester;
    GameObject bgmRequester;

    public  SoundManagerUpdater soundManagerUpdater;

    public string[] audioTypeList = { "BGM_SOUND", "EFFECT_SOUND", "VOCIE_SOUND" };
    private const float soundNomalizer = 10.0f;
    private Dictionary<string, string> audioTypeDict = new Dictionary<string, string>();

    public void CreateSoundManager()
    {
       // SettingManager.Instance.ReadSettingFile();

        if (null != audioSourceList)
        {
            DebugManager.Instance.PrintDebug("Already Created Dafault AudioSources!");
            return;
        }
        audioSourceList = new Dictionary<string, AudioSource>();



        soundManager = GameObject.Find("SoundManager");

        if(soundManager==null)
        {
            soundManager = new GameObject("SoundManager");
            soundManager.AddComponent<SoundManagerUpdater>();
            soundManagerUpdater = soundManager.GetComponent<SoundManagerUpdater>();
            DebugManager.Instance.PrintDebug(soundManager != null, "create new SoundManager GameObject");
        }
        GameObject.DontDestroyOnLoad(soundManager);

        bgmRequester = GameObject.Find("BGMRequester");
        if (bgmRequester != null)
        {
            DebugManager.Instance.PrintDebug("찾음");
            (bgmRequester.gameObject.GetComponent<SoundRequesterBGM>()).RequestShootSound();
        }
        else
        {
            DebugManager.Instance.PrintDebug("응 없어");
        }


    }



    public bool AddAudioSource(string audioSourceKey, AudioSource audioSource,string sourceSetter)
    {
        GameObject gameManager = GameObject.Find("SoundManager");


        if (audioTypeDict.ContainsKey(sourceSetter+"@"+audioSourceKey) == true)
        {
            DebugManager.Instance.PrintDebug("Already Registed AudioSource! AudioSource= " + sourceSetter + "@"+audioSourceKey);
        }
        else
        {
            audioTypeDict.Add(sourceSetter + "@" + audioSourceKey, sourceSetter);
            audioSourceList.Add(audioSourceKey, audioSource);


            return true;
        }
        return false;
    }


    public void SetAudioSound(int soundVol, string audioSourceKey)
    {
        if (audioSourceList.ContainsKey(audioSourceKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist AudioSource! AudioSource= " + audioSourceKey);
            
            return;
        }

        audioSourceList[audioSourceKey].volume = soundVol / soundNomalizer * SettingManager.Instance.GetSettingValue(SettingManager.TOTAL_SOUND);
    }

    public void SetAllAudioSound(int soundVol)
    {
        foreach (KeyValuePair<string, AudioSource> items in audioSourceList)
        {
            audioSourceList[items.Key].volume = soundVol / soundNomalizer * SettingManager.Instance.GetSettingValue(SettingManager.TOTAL_SOUND);
        }
    }


    public void PlayAudioClip(string audioSourceKey, AudioClip audioClip, bool shootType = true, bool isLoop = false)
    {
        audioSourceKey = GetSpeakerNameWithType(audioSourceKey);

        if (audioSourceList.ContainsKey( audioSourceKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist audioSourceKey! audioSourceKey= " + audioSourceKey);
            return;
        }


        DebugManager.Instance.PrintDebug("Shoot Sound with AudioSourceKey= " + audioSourceKey);


        if (shootType) {
            audioSourceList[audioSourceKey].Stop();
            audioSourceList[audioSourceKey].PlayOneShot(audioClip);

        }
        else { 
            audioSourceList[audioSourceKey].Stop();
            audioSourceList[audioSourceKey].loop = isLoop;
            audioSourceList[audioSourceKey].clip = audioClip;
            audioSourceList[audioSourceKey].Play();
        }


    }

    public void RequestSetCallBack(AudioSource targetSpeaker, SoundRequester soundRequester, PackItem packItem){
        soundManagerUpdater.AddRequestSource(targetSpeaker,soundRequester,packItem);
    }

    public AudioSource GetAudioSource(string speakerName) { 
        if (audioSourceList.ContainsKey(GetSpeakerNameWithType(speakerName))) {
            return audioSourceList[GetSpeakerNameWithType(speakerName)];
        }
        return null;
    }


    public List<string> GetAudioSourceID()
    {
        return audioSourceList.Keys.ToList();
    }

    public float GetSettingSound(string audioType) { 
  
            return SettingManager.Instance.GetSettingValue(audioType) / soundNomalizer * SettingManager.Instance.GetSettingValue(SettingManager.TOTAL_SOUND) / soundNomalizer;
    }

    public void PauseAll() { 
        foreach (string key in audioSourceList.Keys) {
            if (audioSourceList[key] != null)
                audioSourceList[key].Pause();
            DebugManager.Instance.PrintDebug("SoundManager : Pause "+ key);

        }

    }
    public void PlayAll()
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (audioSourceList[key] != null)
                audioSourceList[key].Play();
            DebugManager.Instance.PrintDebug("SoundManager : Play " + key);
        }

    }

    public void UnPauseAll()
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (audioSourceList[key] != null)
                audioSourceList[key].UnPause();
            DebugManager.Instance.PrintDebug("SoundManager : UnPause " + key);

        }

    }
    public void PauseType(EAudioType audioType)
    {
        foreach (string key in audioTypeDict.Keys)
        {
           if (key.Split('@')[0] == audioTypeList[(int)audioType])
            {
                if(audioSourceList[key.Split('@')[1]]!=null)
                    audioSourceList[key.Split('@')[1]].Pause();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Pause " + key);

        }

    }
    public void PauseType(string audioType)
    {
        foreach (string key in audioTypeDict.Keys)
        {
            if (key.Split('@')[0].Equals(audioType))
            {
                if (audioSourceList[key.Split('@')[1]] != null)
                    audioSourceList[key.Split('@')[1]].Pause();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Pause " + key);

        }

    }

    public void PlayType(EAudioType audioType)
    {
        foreach (string key in audioTypeDict.Keys)
        {
            if (key.Split('@')[0] == audioTypeList[(int)audioType])
            {
                if (audioSourceList[key.Split('@')[1]] != null)
                    audioSourceList[key.Split('@')[1]].Play();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Play " + key);
        }

    }
    public void PlayType(string audioType)
    {
        foreach (string key in audioTypeDict.Keys)
        {
            if (key.Split('@')[0].Equals(audioType))
            {
                if (audioSourceList[key.Split('@')[1]] != null)
                    audioSourceList[key.Split('@')[1]].Play();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Play " + key);
        }

    }

    public void ResetVolume()
    {
        foreach (string key in audioTypeDict.Keys)
        {
            if (key.Split('@')[0] == audioTypeList[(int)EAudioType.BGM])
            {
                if (audioSourceList[key.Split('@')[1]] != null) { 
                    audioSourceList[key.Split('@')[1]].volume= GetSettingSound(audioTypeList[(int)EAudioType.BGM]);
                    DebugManager.Instance.PrintDebug("SoundManager : ResetVolume " + key +" "+ EAudioType.BGM.ToString());
                }
            }
            else if (key.Split('@')[0] == audioTypeList[(int)EAudioType.EFFECT])
            {
                if (audioSourceList[key.Split('@')[1]] != null) { 
                    audioSourceList[key.Split('@')[1]].volume = GetSettingSound(audioTypeList[(int)EAudioType.EFFECT]);
                    DebugManager.Instance.PrintDebug("SoundManager : ResetVolume " + key + " " + EAudioType.EFFECT.ToString());
                }

            }
            else if(key.Split('@')[0] == audioTypeList[(int)EAudioType.VOICE])
            {
                if (audioSourceList[key.Split('@')[1]] != null) { 
                    audioSourceList[key.Split('@')[1]].volume = GetSettingSound(audioTypeList[(int)EAudioType.VOICE]);
                    DebugManager.Instance.PrintDebug("SoundManager : ResetVolume " + key + " " + EAudioType.VOICE.ToString());
                }

            }

        }

    }


    public void UnPauseType(EAudioType audioType)
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (key.Split('@')[0] == audioTypeList[(int)audioType])
            {
                audioSourceList[key].UnPause();
            }
            DebugManager.Instance.PrintDebug("SoundManager : UnPause " + key);

        }

    }
    public void UnPauseType(string audioType)
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (key.Split('@')[0].Equals(audioType))
            {
                audioSourceList[key].UnPause();
            }
            DebugManager.Instance.PrintDebug("SoundManager : UnPause " + key);

        }

    }

    public GameObject GetSoundManagerGameObject() { 
        return soundManager;
    }

    public GameObject GetSoundManagerRequesterGameObject()
    {
        return soundManager;
    }

    public string GetSpeakerNameWithType(string speakerName) {
        if (audioTypeDict.ContainsKey(speakerName)) {
            return audioTypeDict[speakerName]+"@"+speakerName;
        }
        else { 
            return speakerName;
        }
    }



    public void RefindBGMRequester() {
        bgmRequester = GameObject.Find("BGMRequester");
        if (bgmRequester != null)
        {
            DebugManager.Instance.PrintDebug("찾음");
            bgmRequester.gameObject.GetComponent<SoundRequesterBGM>().RequestShootSound();
        }
        else
        {
            DebugManager.Instance.PrintDebug("응 없어");
        }

    }

   
     

}
