using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerUpdater : MonoBehaviour
{
    class RequestedSource
    {
        public AudioSource targetSpeaker;
        public SoundRequester soundRequester;
        public bool preStat = false;
        public PackItem packItem;

        public RequestedSource(AudioSource targetSpeaker, SoundRequester soundRequester, PackItem packItem)
        {
            this.targetSpeaker = targetSpeaker;
            this.soundRequester = soundRequester;
            this.packItem = packItem;
        }
    }

    Dictionary<string, RequestedSource> requestedDict = new Dictionary<string, RequestedSource>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        foreach (string speakerName in requestedDict.Keys)
        {
            if (requestedDict[speakerName].preStat && !requestedDict[speakerName].targetSpeaker.isPlaying)
            {
                requestedDict[speakerName].soundRequester.RequestCallBack(requestedDict[speakerName].packItem);
                requestedDict.Remove(speakerName);
                if(requestedDict.Count == 0) { 
                    break;
                }

            }
            else
            {
                requestedDict[speakerName].preStat = requestedDict[speakerName].targetSpeaker.isPlaying;
            }
        }

        //   DebugManager.Instance.PrintDebug("Update Now");
    }

    public void AddRequestSource(AudioSource targetSpeaker, SoundRequester soundRequester, PackItem packItem)
    {

        requestedDict.Add(packItem.usingSpeaker, new RequestedSource(targetSpeaker, soundRequester, packItem));
    }
    public void ReturnBackAfterShooting(GameObject parentRequester, GameObject target, float duration)
    {
        StartCoroutine(DestroyAudioAfterPlaying(parentRequester, target, duration));
    }
    protected IEnumerator DestroyAudioAfterPlaying(GameObject parentRequester, GameObject target, float duration)
    {
        yield return new WaitForSeconds(duration);
        DebugManager.Instance.PrintDebug("[SoundRequester] Return Back AudioSource " + duration);
        target.transform.SetParent(parentRequester.transform);
    }
}
