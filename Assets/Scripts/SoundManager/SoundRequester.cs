using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundRequester : MonoBehaviour
{
    // Start is called before the first frame update


 
    

    [ExecuteInEditMode]
    public AudioClip shootingSound;
    public AudioSourceSetter sourceSetter;

    public string soundClipName = "Have To Write";
    public string soundShooterName = "BASE";
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }


    public bool AddAudioClipToManager() {
        if (soundClipName.Equals("Have To Write"))
        {
            DebugManager.Instance.PrintDebug("Have To AudioClipName");
            return false;

        }
 

        return SoundManager.Instance.AddAudioClip(soundClipName, shootingSound);
    }

    public void RequestShootSound() {
        if (soundShooterName.Equals("BASE"))
        {
            soundShooterName += sourceSetter.isLoop ? "_LOOP_" + sourceSetter.audioType : "_ONE_" + sourceSetter.audioType;
        }
   
        SoundManager.Instance.AddAudioSource(soundShooterName, sourceSetter.isLoop, sourceSetter);

        AddAudioClipToManager();
        SoundManager.Instance.PlayAudioClip(soundClipName, soundShooterName);
    }

}
