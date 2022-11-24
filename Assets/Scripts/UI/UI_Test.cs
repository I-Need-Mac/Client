using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// UI테스트용
public class UI_Test : MonoBehaviour
{
    private void Awake()
    {
        SettingManager.Instance.ReadSettingFile();
        SoundManager.Instance.CreateSoundManager();

        // manager init
        UIManager.Instance.Init();
    }

    void Start()
    {
    }
}