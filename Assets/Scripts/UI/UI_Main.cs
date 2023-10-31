using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// UI테스트용
public class UI_Main : MonoBehaviour
{
    private void Awake()
    { 
        SoundManager.Instance.CreateSoundManager();

        // manager init
        UIManager.Instance.Init();
    }

    void Start()
    {

    }



    private void Update()
    {
       
    }
}