using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    Dictionary<string, AudioClip> audioClipsList = new Dictionary<string, AudioClip>();
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

    public bool AddAudioClip(string audioKey, AudioClip audioClip)
    {
        DebugManager.Instance.PrintDebug(audioClip != null, "Invalid AudioClip! AudioKey= " + audioKey.ToString());

        if (audioClipsList.ContainsKey(audioKey) == true)
        {
            DebugManager.Instance.PrintDebug("Already Registed AudioClip! AudioKey= " + audioKey.ToString());
            return false;
        }
        audioClipsList.Add(audioKey, audioClip);

        return true;

    }

    public bool AddAudioSource(string audioSourceKey, bool isLoop, AudioSourceSetter audioSetting)
    {
        GameObject gameManager = GameObject.Find("SoundManager");


        if (audioSourceList.ContainsKey(audioSourceKey) == true)
        {
            DebugManager.Instance.PrintDebug("Already Registed AudioSource! AudioSource= " + audioSourceKey);
        }
        else
        {

            audioSourceList.Add(audioSourceKey, gameManager.AddComponent<AudioSource>());
            audioSourceList[audioSourceKey].loop = isLoop;
            audioSourceList[audioSourceKey].volume = SettingManager.Instance.GetSettingValue(audioSetting.audioType) / soundNomalizer;

            audioSourceList[audioSourceKey].bypassEffects = audioSetting.isBypassEffects;
            audioSourceList[audioSourceKey].priority = audioSetting.priority;
            audioSourceList[audioSourceKey].pitch = audioSetting.pitch;
            audioSourceList[audioSourceKey].panStereo = audioSetting.streoPan;
            audioSourceList[audioSourceKey].outputAudioMixerGroup = audioSetting.audioMixerGroup;

            audioSourceList[audioSourceKey].volume = audioSetting.volume;
            audioSourceList[audioSourceKey].spatialBlend = audioSetting.spatialBlend;

            return true;
        }
        return false;
    }


    public void SetAudioSound(int soundVol, string audioSourceKey)
    {
        if (audioClipsList.ContainsKey(audioSourceKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist AudioSource! AudioSource= " + audioSourceKey);
            return;
        }

        audioSourceList[audioSourceKey].volume = soundVol / soundNomalizer;
    }

    public void SetAllAudioSound(int soundVol)
    {
        foreach (KeyValuePair<string, AudioSource> items in audioSourceList)
        {
            audioSourceList[items.Key].volume = soundVol / soundNomalizer;
        }
    }


    public void PlayAudioClip(string audioKey, string audioSourceKey)
    {
        if (audioClipsList.ContainsKey(audioKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist AudioClip! AudioKey= " + audioKey);
            return;
        }
        if (audioSourceList.ContainsKey(audioSourceKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist audioSourceKey! audioSourceKey= " + audioSourceKey);
            return;
        }


        DebugManager.Instance.PrintDebug("Shoot Sound with AudioSourceKey= " + audioSourceKey);
        DebugManager.Instance.PrintDebug("Shoot Sound with AudioKey= " + audioKey);


        audioSourceList[audioSourceKey].Stop();
        audioSourceList[audioSourceKey].clip = audioClipsList[audioKey];
        audioSourceList[audioSourceKey].Play();



    }


    public List<string> GetAudioSourceID()
    {
        return audioSourceList.Keys.ToList();
    }
}
