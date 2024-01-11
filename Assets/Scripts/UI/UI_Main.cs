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

    // 해상도를 변경합니다

    void Start()
    {

    }



    private void Update()
    {
       
    }
}