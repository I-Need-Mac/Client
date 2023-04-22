using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequesterBGM : SoundRequester
{


    public Dictionary<BGMSituation.BGMSITUATION, BGMPackItem> shootingSounds = new Dictionary<BGMSituation.BGMSITUATION, BGMPackItem>();


    [ExecuteInEditMode]
    public List<AudioSourceSetter> speakerSettings = new List<AudioSourceSetter>();
    public List<BGMPackItem> soundPackItems = new List<BGMPackItem>();



    private void Awake()
    {
      



    }
    void Start()
    {   
        MakeSpeakers();
        ConvertAudioClipData();

        ChangeSituation(BGMSituation.BGMSITUATION.CHANGE_SCENE);

    }

    private void OnEnable()
    {
        ChangeSituation(BGMSituation.BGMSITUATION.CHANGE_SCENE);
    }

    // Update is called once per frame
    void Update()
    {



    }



    public override bool MakeSpeakers()
    {
        soundRequester = new GameObject("SoundRequester");
        soundRequester.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        soundRequester.transform.SetParent(this.transform);


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
                audioSources[items.speakerName].volume = SoundManager.Instance.getSettingSound(items.audioType) * items.volume;
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
    public override void ConvertAudioClipData()
    {
        foreach (BGMPackItem items in soundPackItems)
        {

            if (shootingSounds.ContainsKey(items.BGMSITUATION))
            {
                DebugManager.Instance.PrintDebug("SoundPack : OverWrite SoundSource Data " + items.usingSpeaker);
                shootingSounds[items.BGMSITUATION] = items;
            }
            else
            {
                DebugManager.Instance.PrintDebug("SoundPack : SoundSource Data " + items.usingSpeaker);
                shootingSounds.Add(items.BGMSITUATION, items);
            }

            if (!audioSources.ContainsKey(items.usingSpeaker))
            {
                DebugManager.Instance.PrintDebug("SoundPack : SoundSource From SoundManager " + items.usingSpeaker);
                audioSources.Add(items.usingSpeaker, SoundManager.Instance.GetAudioSource(items.usingSpeaker));

            }
        }

    }

    public override void ChangeSituation(BGMSituation.BGMSITUATION situation)
    {


        if (shootingSounds.ContainsKey(situation))
        {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource Call " + situation + " With " + shootingSounds[situation].usingSpeaker);
            if (shootingSounds[situation].delay == 0)
            {
                ShootSound(shootingSounds[situation].usingSpeaker, shootingSounds[situation].introBGMClip);
                RequestShootSound(audioSources[shootingSounds[situation].usingSpeaker], shootingSounds[situation]);
            }
            else
            {
                StartCoroutine(PlaySoundWithDelay(situation, shootingSounds[situation].introBGMClip, shootingSounds[situation].delay));
                RequestShootSound(audioSources[shootingSounds[situation].usingSpeaker], shootingSounds[situation]);
            }
        }
        else
        {
            DebugManager.Instance.PrintDebug("SoundRequester : SoundSource didn't set " + situation);
        }
    }


    protected override void ShootSound(string speakName, AudioClip clip)
    {
        audioSources[speakName].clip = clip;
        audioSources[speakName].Play();

     

    }
    public override void RequestShootSound() { 
    }
    public override void RequestShootSound(AudioSource targetSpeaker, BGMPackItem targetItem)
    {
        SoundManager.Instance.RequestSetCallBack(targetSpeaker,this,targetItem);

    }

    
    public override void RequestCallBack(PackItem item) {
        BGMPackItem covertItem = (BGMPackItem)item;
        if (covertItem.delayWithIntro == 0) {
            audioSources[covertItem.usingSpeaker].loop = true;
            ShootSound(covertItem.usingSpeaker, covertItem.realBGMClip);
        }
        else {
            audioSources[covertItem.usingSpeaker].loop = true;
            StartCoroutine(PlaySoundWithDelay(covertItem.usingSpeaker, covertItem.realBGMClip, covertItem.delayWithIntro));
        }
    }



    protected override IEnumerator PlaySoundWithDelay(BGMSituation.BGMSITUATION situation, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootSound(shootingSounds[situation].usingSpeaker, clip);
    }
    protected override IEnumerator PlaySoundWithDelay(string speakerName, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootSound(speakerName, clip);
    }



    public override void ChangeSituation(SoundSituation.SOUNDSITUATION situation)
    {
        throw new System.NotImplementedException();
    }

    protected override void ShootSound(SoundSituation.SOUNDSITUATION situation)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator PlaySoundWithDelay(SoundSituation.SOUNDSITUATION situation, float delay)
    {
        throw new System.NotImplementedException();
    }

  
}
