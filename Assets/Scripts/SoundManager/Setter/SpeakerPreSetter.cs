using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerPreSetter : MonoBehaviour
{

    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    private GameObject soundRequester = null;

    [ExecuteInEditMode]
    public List<AudioSourceSetter> speakerSettings = new List<AudioSourceSetter>();
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        MakeSpeakers();
    }

    //private void OnEnable()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    public void preInit() { 
    
      //  MakeSpeakers();
    }
    private bool MakeSpeakers()
    {
        GameObject soundManager = SoundManager.Instance.GetSoundManagerGameObject();
        try {
            soundRequester = soundManager.transform.Find("SoundRequester").gameObject;
        }catch(NullReferenceException e) {
            soundRequester = new GameObject("SoundRequester");
            soundRequester.transform.position = new Vector3(soundManager.transform.position.x, soundManager.transform.position.y, soundManager.transform.position.z);
            soundRequester.transform.SetParent(soundManager.transform);
        }

        foreach (AudioSourceSetter items in speakerSettings)
        {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource 생성 " + items.speakerName);



            if (items.loadSettingFrom.Equals(null))
            {

                GameObject speaker = new GameObject(items.speakerName);
                speaker.transform.position = new Vector3(soundRequester.transform.position.x, soundRequester.transform.position.y, soundRequester.transform.position.z);
                speaker.transform.SetParent(soundRequester.transform);


                audioSources.Add(items.speakerName, speaker.AddComponent<AudioSource>());
                SoundManager.Instance.AddAudioSource(items.speakerName, audioSources[items.speakerName]);

                audioSources[items.speakerName].loop = items.isLoop;
                audioSources[items.speakerName].volume = SoundManager.Instance.GetSettingSound(items.audioType) * items.volume;
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
            else
            {

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
}
