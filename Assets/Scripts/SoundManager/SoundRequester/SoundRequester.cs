
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SoundRequester : MonoBehaviour
{
    // Start is called before the first frame update

    public int soundObjectID { get; set;}

    protected Dictionary<string,AudioSource> audioSources = new Dictionary<string, AudioSource>();
    protected GameObject soundRequester;
  



    


 
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



    public abstract bool MakeSpeakers();

    public abstract void ConvertAudioClipData();


    public abstract void ChangeSituation(SoundSituation.SOUNDSITUATION situation);
    public abstract void ChangeSituation(BGMSituation.BGMSITUATION situation);

    protected abstract void ShootSound(SoundSituation.SOUNDSITUATION situation);
    protected abstract void ShootSound(string speakName, AudioClip clip);

    public abstract void RequestShootSound();
    public abstract void RequestShootSound(AudioSource targetSpeaker, BGMPackItem targetItem);


    public abstract void RequestCallBack(PackItem item);

    protected abstract IEnumerator PlaySoundWithDelay(SoundSituation.SOUNDSITUATION situation, float delay);
    protected abstract IEnumerator PlaySoundWithDelay(BGMSituation.BGMSITUATION situation, AudioClip clip, float delay);
    protected abstract IEnumerator PlaySoundWithDelay(string speakerName, AudioClip clip, float delay);
}
