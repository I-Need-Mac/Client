
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SoundRequester : MonoBehaviour
{
    // Start is called before the first frame update

    public String soundObjectID { get; set;}

    protected Dictionary<string,AudioSource> audioSources = new Dictionary<string, AudioSource>();
    protected GameObject soundRequester;
    protected GameObject soundManager = SoundManager.Instance.GetSoundManagerGameObject();
    protected List<GameObject> givenSpeaker = new List<GameObject>();
    protected SoundSituation.SOUNDSITUATION lastSoundSituation = SoundSituation.SOUNDSITUATION.NONE;
    protected bool isBlockSituation = false;
    
    public bool isBlockAllSituation = false;
    public float situationBlockTime = 0.5f;






    private void Awake()
    {
        DebugManager.Instance.PrintDebug("[SoundRequester] init");
        Guid guid = Guid.NewGuid();
        soundObjectID = guid.ToString();

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

    public abstract bool isPlaying(SoundSituation.SOUNDSITUATION situation);
    protected abstract IEnumerator PlaySoundWithDelay(SoundSituation.SOUNDSITUATION situation, float delay);
    protected abstract IEnumerator PlaySoundWithDelay(BGMSituation.BGMSITUATION situation, AudioClip clip, float delay);
    protected abstract IEnumerator PlaySoundWithDelay(string speakerName, AudioClip clip, float delay);

    protected void MoveToSoundManager(GameObject target) {
        AudioSource audioSource = target.GetComponent<AudioSource>();
        if (audioSource.clip != null){
            if (soundManager == null) { 
                soundManager = SoundManager.Instance.GetSoundManagerGameObject();
            }
       
          
            target.transform.SetParent(soundManager.transform);

            givenSpeaker.Add(target);

            //SoundManager.Instance.soundManagerUpdater.ReturnBackAfterShooting(parentRequester, target, audioSource.clip.length);
        }
    }

    public void GetBackSpeakers() { 
        if (givenSpeaker.Count != 0) { 
            for(int i = 0; i< givenSpeaker.Count; ++i) {
                givenSpeaker[i].transform.position = new Vector3(soundRequester.transform.position.x, soundRequester.transform.position.y, soundRequester.transform.position.z);
                givenSpeaker[i].transform.SetParent(soundRequester.transform);
                givenSpeaker.RemoveAt(i);
                i-=1;
            } 
        }
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

    protected void BlockSituation() {
        isBlockSituation = true;
        StartCoroutine(UnlockBlockSituation());
    }
    protected IEnumerator UnlockBlockSituation()
    {
        yield return new WaitForSeconds(situationBlockTime);
        isBlockSituation = false;

    }
}
