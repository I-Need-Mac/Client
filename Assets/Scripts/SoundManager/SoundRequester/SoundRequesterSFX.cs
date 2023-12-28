using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequesterSFX : SoundRequester
{
    public Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem> shootingSounds = new Dictionary<SoundSituation.SOUNDSITUATION, SoundPackItem>();
    public Dictionary<string, GameObject> audioGameObjectDict = new Dictionary<string, GameObject>();

    [ExecuteInEditMode]
    public List<AudioSourceSetter> speakerSettings = new List<AudioSourceSetter>();
    public List<SoundPackItem> soundPackItems = new List<SoundPackItem>();
  

    private SoundSituation.SOUNDSITUATION lastSituation;

    public override bool MakeSpeakers()
    {

        soundRequester = new GameObject("SoundRequester");
        soundRequester.SetActive(true);
        soundRequester.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        soundRequester.transform.SetParent(this.transform);


        foreach (AudioSourceSetter items in speakerSettings)
        {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource 생성 " + items.audioType + "@" + soundObjectID + "!" + items.speakerName);



            if (items.loadSettingFrom == null)
            {
                DebugManager.Instance.PrintDebug("SoundRequester : "+ items.audioType + "@" + soundObjectID + "!" + items.speakerName + " Make New Speaker ");

               GameObject speaker = new GameObject(items.speakerName);
                speaker.transform.position = new Vector3(soundRequester.transform.position.x, soundRequester.transform.position.y, soundRequester.transform.position.z);
                speaker.transform.SetParent(soundRequester.transform);


                audioSources.Add(items.speakerName, speaker.AddComponent<AudioSource>());
                audioGameObjectDict.Add(items.speakerName, speaker);

                SoundManager.Instance.AddAudioSource(soundObjectID + "!" + items.speakerName, audioSources[items.speakerName], items.audioType);

                audioSources[items.speakerName].loop = items.isLoop;
                audioSources[items.speakerName].volume = SoundManager.Instance.GetSettingSound(items.audioType) ;
                audioSources[items.speakerName].playOnAwake = false;

                audioSources[items.speakerName].bypassEffects = items.isBypassEffects;
                audioSources[items.speakerName].priority = items.priority;
                audioSources[items.speakerName].pitch = items.pitch;
                audioSources[items.speakerName].panStereo = items.streoPan;
                audioSources[items.speakerName].outputAudioMixerGroup = items.audioMixerGroup;

                audioSources[items.speakerName].spatialBlend = items.spatialBlend;
                audioSources[items.speakerName].dopplerLevel = items.dropperLevel;
                audioSources[items.speakerName].spread = items.spread;
                audioSources[items.speakerName].minDistance = items.minDistance;
                audioSources[items.speakerName].maxDistance = items.maxDistance;
            }
            else
            {
                DebugManager.Instance.PrintDebug("SoundRequester : " + items.audioType + "@" + soundObjectID + "!" + items.speakerName + " Load Speaker ");
                GameObject speaker = Instantiate(items.loadSettingFrom);

                speaker.name = items.speakerName;
                speaker.transform.position = new Vector3(soundRequester.transform.position.x, soundRequester.transform.position.y, soundRequester.transform.position.z);
                speaker.transform.SetParent(soundRequester.transform);

                audioSources.Add(items.speakerName, speaker.GetComponent<AudioSource>());
                audioGameObjectDict.Add(items.speakerName, speaker);
                SoundManager.Instance.AddAudioSource(soundObjectID + "!" + items.speakerName, audioSources[items.speakerName], items.audioType);

            }

        }
        return true;
    }

    public override void ConvertAudioClipData()
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

    public override void ChangeSituation(SoundSituation.SOUNDSITUATION situation)
    {


        if (shootingSounds.ContainsKey(situation))
        {
            if (lastSituation == SoundSituation.SOUNDSITUATION.NONE)
            {
                lastSituation = situation;
            }
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource Call " + situation + " With " + shootingSounds[situation].usingSpeaker);
            if (shootingSounds[lastSituation].priority <= shootingSounds[situation].priority)
            {
                if (shootingSounds[situation].delay == 0)
                {
                    ShootSound(situation);
                }
                else
                {
                    StartCoroutine(PlaySoundWithDelay(situation, shootingSounds[situation].delay));
                }
            }
        }
        else
        {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource didn't set " + situation);
        }
    }

    protected override void ShootSound(SoundSituation.SOUNDSITUATION situation)
    {
        //audioSources[shootingSounds[situation].usingSpeaker].clip = shootingSounds[situation].audioClip;
       
        try {
            audioSources[shootingSounds[situation].usingSpeaker].PlayOneShot(shootingSounds[situation].audioClip);
        }
        catch(Exception e) { 
            DebugManager.Instance.PrintError("[SoundRequester] 요청 사운드 이상 "+ situation+ " "+shootingSounds[situation].usingSpeaker);
        
        }
        if (situation == SoundSituation.SOUNDSITUATION.DIE)
        {
           MoveToSoundManager(audioGameObjectDict[shootingSounds[situation].usingSpeaker]);
        }

    }



    protected override void ShootSound(string speakName, AudioClip clip)
    {
        audioSources[speakName].clip = clip;
        audioSources[speakName].Play();
    }
    public override bool isPlaying(SoundSituation.SOUNDSITUATION situation)
    {
        if (shootingSounds.ContainsKey(situation))
        {
            return audioSources[shootingSounds[situation].usingSpeaker].isPlaying;
        }
        else
        {
            return false;
        }

    }

    public override void RequestShootSound()
    {
    }

    protected override IEnumerator PlaySoundWithDelay(SoundSituation.SOUNDSITUATION situation, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootSound(situation);
    }

    public override void ChangeSituation(BGMSituation.BGMSITUATION situation)
    {
        throw new System.NotImplementedException();
    }

    public override void RequestShootSound(AudioSource targetSpeaker, BGMPackItem targetItem)
    {
        throw new System.NotImplementedException();
    }

    public override void RequestCallBack(PackItem item)
    {
        throw new System.NotImplementedException();
    }



    protected override IEnumerator PlaySoundWithDelay(BGMSituation.BGMSITUATION situation, AudioClip clip, float delay)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator PlaySoundWithDelay(string speakerName, AudioClip clip, float delay)
    {
        throw new System.NotImplementedException();
    }
}
