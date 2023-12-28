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
        SetScreenSize();
        SetScreenType();

    }
    public void SetScreenType()
    {
            switch (SettingManager.Instance.GetSettingValue("ScreenType"))
            {
                case 0:
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2:
                    Screen.fullScreen = false;
                    break;

            }
    }
    // 해상도를 변경합니다
    public void SetScreenSize()
    {
            Screen.SetResolution(SettingManager.Instance.GetSettingValue("ScreenWidth"), SettingManager.Instance.GetSettingValue("ScreenHeight"), true, SettingManager.Instance.GetSettingValue("ScreenRate"));
    }
    void Start()
    {

    }



    private void Update()
    {
       
    }
}