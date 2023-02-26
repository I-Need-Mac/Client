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

    public string soundShooterName = "BASE";
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        


    }



    public void RequestShootSound() {
        if (soundShooterName.Equals("BASE"))
        {
            soundShooterName += sourceSetter.isLoop ? "_LOOP_" + sourceSetter.audioType : "_ONE_" + sourceSetter.audioType;
        }
   
        SoundManager.Instance.AddAudioSource(soundShooterName, sourceSetter.isLoop, sourceSetter);

        SoundManager.Instance.PlayAudioClip(soundShooterName,shootingSound);
    }

}
