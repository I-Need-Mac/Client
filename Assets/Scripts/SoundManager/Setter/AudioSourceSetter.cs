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
    [Tooltip("스피커 이름 - 동일한 이름을 사용할 경우 사운드가 교체됨")]
    public string speakerName = "Default";

    [Tooltip("스피커 타입 - 해당 타입에 따라 사운드 조절 설정에 따를 종류가 정해짐")]
    public EAudioType _audioType;

    [Tooltip("로딩할 스피커 - 여기에 들어가는 옵션과 그대로 스피커를 생성")]
    public GameObject loadSettingFrom;

    public AudioMixerGroup audioMixerGroup;

    [Tooltip("반복 여부")]
    public bool isLoop = false;
    [Tooltip("몰루")]
    public bool isBypassEffects = false;
    

    [Tooltip("128이 기본")]
    [Range(0, 256)] public int priority = 128;
    [Tooltip("1이 기본")]
    [Range(0, 3)] public int pitch = 1;
    [Range(-1, 1)] public int streoPan = 0;

    [Range(0, 1)] public float volume = 1;

    [Tooltip("3D 스피커 생성 시 loadSettingFrom 사용 권장")]
    public bool is3D = false;
    public AudioRolloffMode  audioRolloffMode;
    [Range(0, 5)] public float dropperLevel = 0f;
    [Range(0, 360)] public float spread = 0f;
    public float minDistance = 0f;
    public float maxDistance = 0f;

    public float spatialBlend {
        get {
            if (is3D) { 
                return 1;
            }
            else { 
                return 0;
            }
        }
    }

    private string[] audioTypeList = { "BGM_SOUND", "EFFECT_SOUND", "VOCIE_SOUND" };

    public string audioType
    {
        get { 
            return audioTypeList[(int)_audioType];
        }
    }




}
