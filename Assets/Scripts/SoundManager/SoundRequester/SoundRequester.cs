
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
        DebugManager.Instance.PrintDebug("[SoundRequester] init");
        MakeSpeakers();
        ConvertAudioClipData();




    }
    void Start()
    {



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

    protected void MoveToSoundManager(GameObject target) {
        GameObject soundManager = GameObject.Find("SoundManager");
        target.transform.position = new Vector3(soundManager.transform.position.x, soundManager.transform.position.y, soundManager.transform.position.z);
        target.transform.SetParent(soundManager.transform);
        
        AudioSource audioSource = target.GetComponent<AudioSource>();
        StartCoroutine(DestroyAudioAfterPlaying(audioSource, audioSource.clip.length));
    }


    protected IEnumerator DestroyAudioAfterPlaying(AudioSource target, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(target);
    }

    protected GameObject FindChild(GameObject parent, string childName)
    {
        Transform parentTransform = parent.transform;
        Transform childTransform = parentTransform.Find(childName);

        if (childTransform == null)
        {
            Debug.LogWarning("Child with name " + childName + " not found in parent GameObject " + parent.name);
            return null;
        }

        return childTransform.gameObject;
    }
}
