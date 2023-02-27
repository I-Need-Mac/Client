
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SoundRequester : MonoBehaviour
{
    // Start is called before the first frame update



    private Dictionary<string,AudioSource> audioSources = new Dictionary<string, AudioSource>();
    private GameObject soundRequester;
    public Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem> shootingSounds = new Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem>();


    [ExecuteInEditMode]
    public List<AudioSourceSetter> speakerSettings = new List<AudioSourceSetter>();
    public List<SoundPackItem> soundPackItems = new List<SoundPackItem>();
    


 
    private void Awake()
    {

       

        
    }
    void Start()
    {
       

        MakeSpeakers();
        ConvertAudioClipData();

    }

    // Update is called once per frame
    void Update()
    {



    }



    private bool MakeSpeakers() {
        soundRequester = new GameObject("SoundRequester");
        soundRequester.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        soundRequester.transform.SetParent(this.transform);
 

        foreach (AudioSourceSetter items in speakerSettings) {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource 생성 " + items.speakerName);
            
           

            if (items.loadSettingFrom.Equals(null)) {
                GameObject speaker = new GameObject(items.speakerName);
                speaker.transform.position = new Vector3(soundRequester.transform.position.x, soundRequester.transform.position.y, soundRequester.transform.position.z);
                speaker.transform.SetParent(soundRequester.transform);
                

                audioSources.Add(items.speakerName, speaker.AddComponent<AudioSource>());
                SoundManager.Instance.AddAudioSource(items.speakerName, audioSources[items.speakerName]);

                audioSources[items.speakerName].loop = items.isLoop;
                audioSources[items.speakerName].volume = SoundManager.Instance.getSettingSound(items.audioType)*items.volume;
                audioSources[items.speakerName].playOnAwake = false;

                audioSources[items.speakerName].bypassEffects = items.isBypassEffects;
                audioSources[items.speakerName].priority = items.priority;
                audioSources[items.speakerName].pitch = items.pitch;
                audioSources[items.speakerName].panStereo = items.streoPan;
                audioSources[items.speakerName].outputAudioMixerGroup = items.audioMixerGroup;

                audioSources[items.speakerName].volume = items.volume;
                audioSources[items.speakerName].spatialBlend = items.spatialBlend;
                audioSources[items.speakerName].dopplerLevel = items.dropperLevel;
                audioSources[items.speakerName].spread = items.spread;
                audioSources[items.speakerName].minDistance = items.minDistance;
                audioSources[items.speakerName].maxDistance = items.maxDistance;
            }
            else {
 
                GameObject speaker = Instantiate(items.loadSettingFrom);
                
                speaker.name = items.speakerName;
                speaker.transform.position = new Vector3(soundRequester.transform.position.x, soundRequester.transform.position.y, soundRequester.transform.position.z);
                speaker.transform.SetParent(soundRequester.transform);

                audioSources.Add(items.speakerName, speaker.AddComponent<AudioSource>());
                SoundManager.Instance.AddAudioSource(items.speakerName, audioSources[items.speakerName]);

            }

        }
        return true;
    }
    public void ConvertAudioClipData()
    {
        foreach (SoundPackItem items in soundPackItems)
        {

            if (shootingSounds.ContainsKey(items.SOUNDSITUATION))
            {
                DebugManager.Instance.PrintDebug("SoundPack : OverWrite SoundSource Data " + items.usingSpeaker);
                shootingSounds[items.SOUNDSITUATION] = items;
            }
            else
            {
                DebugManager.Instance.PrintDebug("SoundPack : SoundSource Data " + items.usingSpeaker);
                shootingSounds.Add(items.SOUNDSITUATION, items);
            }

            if (!audioSources.ContainsKey(items.usingSpeaker))
            {
                audioSources.Add(items.usingSpeaker, SoundManager.Instance.GetAudioSource(items.usingSpeaker));

            }
        }

    }

    public void ChangeSituation(SoundSituation.SOUNDSITUATION situation) {
       

        if (shootingSounds.ContainsKey(situation)) {
           DebugManager.Instance.PrintDebug("SoundRequester : SoundSource Call " + situation+" With "+shootingSounds[situation].usingSpeaker);
           if (shootingSounds[situation].delay == 0) {
                ShootSound(situation);
            }
            else {
                    StartCoroutine(PlaySoundWithDelay(situation,shootingSounds[situation].delay));
            }
        }
        else {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource didn't set " + situation);
        }
    }


    private void ShootSound(SoundSituation.SOUNDSITUATION situation) {
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

     IEnumerator PlaySoundWithDelay(SoundSituation.SOUNDSITUATION situation, float delay)
    {
        yield return new WaitForSeconds(delay); 
        ShootSound(situation);
    }


}
