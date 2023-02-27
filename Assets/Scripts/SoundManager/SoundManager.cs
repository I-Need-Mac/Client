using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    Dictionary<string, AudioSource> audioSourceList = null;

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



        GameObject gameManager = GameObject.Find("SoundManager");
        {
            gameManager = new GameObject("SoundManager");
            DebugManager.Instance.PrintDebug(gameManager != null, "Can not create new SoundManager GameeObject");
        }
        GameObject.DontDestroyOnLoad(gameManager);

        GameObject bgmRequester = GameObject.Find("BGMRequester");
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

    public AudioSource GetAudioSource(string speakerName) { 
        return audioSourceList[speakerName];
    }


    public List<string> GetAudioSourceID()
    {
        return audioSourceList.Keys.ToList();
    }

    public float getSettingSound(string audioType) { 
        return SettingManager.Instance.GetSettingValue(audioType) / soundNomalizer * SettingManager.Instance.GetSettingValue(SettingManager.TOTAL_SOUND);
    }
}
