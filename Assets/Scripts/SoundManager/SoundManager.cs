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
    GameObject bgmRequester;
    SoundManagerUpdater soundManagerUpdater;
    
    private const float soundNomalizer = 10.0f;
    private string[] audioTypeList = { "BGM_SOUND", "EFFECT_SOUND", "VOCIE_SOUND" };

    public void CreateSoundManager()
    {
        if (null != audioSourceList)
        {
            DebugManager.Instance.PrintDebug("Already Created Dafault AudioSources!");
            return;
        }
        audioSourceList = new Dictionary<string, AudioSource>();



        soundManager = GameObject.Find("SoundManager");
        {
            soundManager = new GameObject("SoundManager");
            soundManager.AddComponent<SoundManagerUpdater>();
            soundManagerUpdater = soundManager.GetComponent<SoundManagerUpdater>();
            DebugManager.Instance.PrintDebug(soundManager != null, "create new SoundManager GameeObject");
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

        AudioSourcePreSetting();
    }



    public bool AddAudioSource(string audioSourceKey, AudioSource audioSource)
    {
        GameObject gameManager = GameObject.Find("SoundManager");


        if (audioSourceList.ContainsKey(audioSourceKey) == true)
        {
            DebugManager.Instance.PrintDebug("Already Registed AudioSource! AudioSource= " + audioSourceKey);
        }
        else
        {

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


    public void PlayAudioClip(string audioSourceKey, AudioClip audioClip)
    {
        if (audioSourceList.ContainsKey(audioSourceKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist audioSourceKey! audioSourceKey= " + audioSourceKey);
            return;
        }


        DebugManager.Instance.PrintDebug("Shoot Sound with AudioSourceKey= " + audioSourceKey);



        audioSourceList[audioSourceKey].Stop();
        audioSourceList[audioSourceKey].clip = audioClip;
        audioSourceList[audioSourceKey].Play();



    }

    public void RequestSetCallBack(AudioSource targetSpeaker, SoundRequester soundRequester, PackItem packItem){
        soundManagerUpdater.AddRequestSource(targetSpeaker,soundRequester,packItem);
    }

    public AudioSource GetAudioSource(string speakerName) { 
        return audioSourceList[speakerName];
    }


    public List<string> GetAudioSourceID()
    {
        return audioSourceList.Keys.ToList();
    }

    public float GetSettingSound(string audioType) { 
        return SettingManager.Instance.GetSettingValue(audioType) / soundNomalizer * SettingManager.Instance.GetSettingValue(SettingManager.TOTAL_SOUND);
    }

    public void PauseAll() { 
        foreach (string key in audioSourceList.Keys) {
    
            audioSourceList[key].Pause();
            DebugManager.Instance.PrintDebug("SoundManager : Pause "+ key);

        }

    }
    public void PlayAll()
    {
        foreach (string key in audioSourceList.Keys)
        {
            audioSourceList[key].Play();
            DebugManager.Instance.PrintDebug("SoundManager : Play " + key);
        }

    }

    public void UnPauseAll()
    {
        foreach (string key in audioSourceList.Keys)
        {
            audioSourceList[key].UnPause();
            DebugManager.Instance.PrintDebug("SoundManager : UnPause " + key);

        }

    }
    public void PauseType(EAudioType audioType)
    {
        foreach (string key in audioSourceList.Keys)
        {
           if (key.Split('@')[0] == audioTypeList[(int)audioType])
            {
                audioSourceList[key].Pause();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Pause " + key);

        }

    }
    public void PauseType(string audioType)
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (key.Split('@')[0].Equals(audioType))
            {
                audioSourceList[key].Pause();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Pause " + key);

        }

    }

    public void PlayType(EAudioType audioType)
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (key.Split('@')[0] == audioTypeList[(int)audioType])
            {
                audioSourceList[key].Play();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Play " + key);
        }

    }
    public void PlayType(string audioType)
    {
        foreach (string key in audioSourceList.Keys)
        {
            if (key.Split('@')[0].Equals(audioType))
            {
                audioSourceList[key].Play();
            }
            DebugManager.Instance.PrintDebug("SoundManager : Play " + key);
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

    public void AudioSourcePreSetting() {
        GameObject preRequster = GameObject.Find("SpeakerPreSetter");
        if (bgmRequester != null)
        {
            DebugManager.Instance.PrintDebug("찾음");
            preRequster.gameObject.GetComponent<SpeakerPreSetter>().preInit();
        }
        else
        {
            DebugManager.Instance.PrintDebug("응 없어");
        }
    }
}
