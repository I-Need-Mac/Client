using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundRequester : MonoBehaviour
{
    // Start is called before the first frame update


    private const string ALONE = "ALONE";
    private Dictionary<string,AudioSource> audioSources;

    public Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem> shootingSounds;
    
    
    [ExecuteInEditMode]
  
    public string soundSpeakerName = ALONE;


    public List<AudioSourceSetter> speakerSettings;
    public List<SoundPackItem> soundPackItems;
  

    void Start()
    {
        makeSpeakers();
        convertAudioClipData();

        if (soundSpeakerName.Equals(ALONE)) {


        }
        else { 
        
        
        
        
        }


    }

    // Update is called once per frame
    void Update()
    {



    }

    private void convertAudioClipData() {
        foreach (SoundPackItem items in soundPackItems)
        {
            shootingSounds.Add(items.SOUNDSITUATION, items);
            if (!audioSources.ContainsKey(items.usingSpeaker))
            {
                audioSources.Add(items.usingSpeaker, SoundManager.Instance.GetAudioSource(items.usingSpeaker));

            }
        }

    }

    private bool makeSpeakers() {
 
        foreach (AudioSourceSetter items in speakerSettings) {
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

    public void ChangeSituation(int situation) { 
        
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
