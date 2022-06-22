using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    Dictionary<string, AudioClip> audioClipsList = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioSource> audioSourceList = null;


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

        audioSourceList.Add("BASE_ONE",gameManager.AddComponent<AudioSource>());
        audioSourceList.Add("BASE_LOOP",gameManager.AddComponent<AudioSource>());

        audioSourceList["BASE_ONE"].loop = false;
        audioSourceList["BASE_LOOP"].loop = true;

    }

    public void AddAudioClip(string audioKey, AudioClip audioClip)
    {
        DebugManager.Instance.PrintDebug(audioClip != null, "Invalid AudioClip! AudioKey= " + audioKey.ToString());

        if (audioClipsList.ContainsKey(audioKey) == true)
        {
            DebugManager.Instance.PrintDebug("Already Registed AudioClip! AudioKey= " + audioKey.ToString());
            return;
        }
        audioClipsList.Add(audioKey, audioClip);

       
    }

    public void AddAudioSource(string audioSourceKey,  bool isLoop)
    {
        GameObject gameManager = GameObject.Find("SoundManager");
        {
            gameManager = new GameObject("SoundManager");
            DebugManager.Instance.PrintDebug(gameManager != null, "Can not create new SoundManager GameeObject");
        }
        GameObject.DontDestroyOnLoad(gameManager);

        DebugManager.Instance.PrintDebug( "Invalid AudioSource AudioSourceKey= " + audioSourceKey);

        if (audioClipsList.ContainsKey(audioSourceKey) == true)
        {
            DebugManager.Instance.PrintDebug("Already Registed AudioSource! AudioSource= " + audioSourceKey);
            return;
        }


        audioSourceList.Add(audioSourceKey, gameManager.AddComponent<AudioSource>());
        audioSourceList[audioSourceKey].loop = isLoop;
      

    }
    public void SetAudioSound(int soundVol, string audioSourceKey) {
        if (audioClipsList.ContainsKey(audioSourceKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist AudioSource! AudioSource= " + audioSourceKey);
            return;
        }

        audioSourceList[audioSourceKey].volume = soundVol;
    }


    public void PlayAudioClip(string audioKey, string audioSourceKey)
    {
        if (audioClipsList.ContainsKey(audioKey) == false)
        {
            DebugManager.Instance.PrintDebug("Not exist AudioClip! AudioKey= " + audioKey);
            return;
        }

    
        DebugManager.Instance.PrintDebug("Shoot Sound with AudioSourceKey= " + audioSourceKey);
        DebugManager.Instance.PrintDebug("Shoot Sound with AudioKey= " + audioKey);

        if (audioSourceList[audioSourceKey].loop)
        {

            audioSourceList[audioSourceKey].Stop();
            audioSourceList[audioSourceKey].clip = audioClipsList[audioKey];
            audioSourceList[audioSourceKey].Play();
        }
        else
        {

            audioSourceList[audioSourceKey].PlayOneShot(audioClipsList[audioKey]);
        }
    }
}
