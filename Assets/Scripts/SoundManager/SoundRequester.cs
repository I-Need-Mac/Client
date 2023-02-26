using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundRequester : MonoBehaviour
{
    // Start is called before the first frame update



    private Dictionary<string,AudioSource> audioSources = new Dictionary<string, AudioSource>();

    public Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem> shootingSounds = new Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem>();
    
    
    [ExecuteInEditMode]
  


    public List<AudioSourceSetter> speakerSettings;
    public List<SoundPackItem> soundPackItems;
  

    void Start()
    {
        MakeSpeakers();
        ConvertAudioClipData();

    }

    // Update is called once per frame
    void Update()
    {



    }

    private void ConvertAudioClipData() {
        foreach (SoundPackItem items in soundPackItems)
        {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource Data " + items.usingSpeaker);
            shootingSounds.Add(items.SOUNDSITUATION, items);
            if (!audioSources.ContainsKey(items.usingSpeaker))
            {
                audioSources.Add(items.usingSpeaker, SoundManager.Instance.GetAudioSource(items.usingSpeaker));

            }
        }

    }

    private bool MakeSpeakers() {
      
        foreach (AudioSourceSetter items in speakerSettings) {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource 생성 " + items.speakerName);
            SoundManager.Instance.AddAudioSource("SS",false, items);
            audioSources.Add(items.speakerName, gameObject.AddComponent<AudioSource>()); 
            audioSources[items.speakerName].loop = items.isLoop;
            audioSources[items.speakerName].volume = SoundManager.Instance.getSettingSound(items.audioType);

            audioSources[items.speakerName].bypassEffects = items.isBypassEffects;
            audioSources[items.speakerName].priority = items.priority;
            audioSources[items.speakerName].pitch = items.pitch;
            audioSources[items.speakerName].panStereo = items.streoPan;
            audioSources[items.speakerName].outputAudioMixerGroup = items.audioMixerGroup;

            audioSources[items.speakerName].volume = items.volume;
            audioSources[items.speakerName].spatialBlend = items.spatialBlend;

        }
        return true;
    }

    public void ChangeSituation(SoundSituation.SOUNDSITUATION situation) {
        DebugManager.Instance.PrintDebug("SoundRequester : SoundSource Call " + shootingSounds[situation].usingSpeaker);
        audioSources[shootingSounds[situation].usingSpeaker].Stop();
        audioSources[shootingSounds[situation].usingSpeaker].clip = shootingSounds[situation].audioClip;
        audioSources[shootingSounds[situation].usingSpeaker].Play();
    }



    public void RequestShootSound()
    {
        /*
        if (soundSpeakerName.Equals("BASE"))
        {
            soundSpeakerName += sourceSetter.isLoop ? "_LOOP_" + sourceSetter.audioType : "_ONE_" + sourceSetter.audioType;
        }

        SoundManager.Instance.AddAudioSource(soundSpeakerName, sourceSetter.isLoop, sourceSetter);

        //SoundManager.Instance.PlayAudioClip(soundSpeakerName, shootingSounds[].audioClip);
        */
    }

}
