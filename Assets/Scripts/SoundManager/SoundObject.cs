using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    
    public SoundRequester soundRequester;

    public void SetSoundRequester(String objectID) {
        soundRequester = GetComponent<SoundRequester>();
        soundRequester.soundObjectID = objectID;
        soundRequester.MakeSpeakers();
        soundRequester.ConvertAudioClipData();
    }
    // Start is called before the first frame update

}
