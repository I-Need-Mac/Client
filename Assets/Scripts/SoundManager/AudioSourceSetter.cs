using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class AudioSourceSetter {
    public enum EAudioType
    {
        BGM,
        EFFECT,
        VOICE
    }
    public AudioMixerGroup audioMixerGroup;
    public EAudioType _audioType;
    public bool isLoop = false;
    public bool isBypassEffects = false;
    

    [Range(0, 256)] public int priority = 128;
    [Range(0, 3)] public int pitch = 1;
    [Range(-1, 1)] public int streoPan = 0;

    private string[] audioTypeList = { "BGM_SOUND", "EFFECT_SOUND", "VOCIE_SOUND" };

    public string audioType
    {
        get { 
            return audioTypeList[(int)_audioType];
        }
    }




}
