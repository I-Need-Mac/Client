using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Settings : UI_Popup
{
    enum Images
    {
        Close,
    }

    [SerializeField] Toggle skipCutScene;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown langDropDown;
    [SerializeField] TMP_Dropdown screenTypeDropDown;
    [SerializeField] Slider total_gauge;
    [SerializeField] Slider bgm_gauge;
    [SerializeField] Slider sfx_gauge;
    [SerializeField] Slider voice_gauge;
    [SerializeField] UI_LocalizeText[] textList;

    List<Resolution> resolutions;

    private int nowSizeIndex=0;
    private int originLang;





    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        originLang = SettingManager.Instance.GetSettingValue("lang");
        langDropDown.value = originLang;
        skipCutScene.isOn = SettingManager.Instance.GetSettingValue("CutScene") > 0;
       

        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }
        SetSoundGauge();
        SetScreenType();
        SetResolution();
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.Close:
                SaveSoundValue();
                SetBootOption();
                this.CloseUI<UI_Settings>();
      
                if (originLang != SettingManager.Instance.GetSettingValue("lang")) { 
                    Application.Quit();
                }
                break;
            default:
                break;
        }
    }
    private void SaveSoundValue()
    {
        SettingManager.Instance.SetSettingValue(SettingManager.TOTAL_SOUND,(int)total_gauge.value);
        SettingManager.Instance.SetSettingValue(SettingManager.BGM_SOUND, (int)bgm_gauge.value);
        SettingManager.Instance.SetSettingValue(SettingManager.EFFECT_SOUND, (int)sfx_gauge.value);
        SettingManager.Instance.SetSettingValue(SettingManager.VOCIE_SOUND, (int)voice_gauge.value);
        SettingManager.Instance.WriteSettingFile();
        SoundManager.Instance.ResetVolume();
    }
    private void SetSoundGauge() {
        total_gauge.value = (float)SettingManager.Instance.GetSettingValue(SettingManager.TOTAL_SOUND);
        bgm_gauge.value = (float)SettingManager.Instance.GetSettingValue(SettingManager.BGM_SOUND);
        sfx_gauge.value = (float)SettingManager.Instance.GetSettingValue(SettingManager.EFFECT_SOUND);
        voice_gauge.value = (float)SettingManager.Instance.GetSettingValue(SettingManager.VOCIE_SOUND);
    }
    void SetResolution()
    {
        string nowSize = SettingManager.Instance.GetSettingValue("ScreenWidth") + " x " + SettingManager.Instance.GetSettingValue("ScreenHeight")+" " + SettingManager.Instance.GetSettingValue("ScreenRate")+"hz";

        SettingManager.Instance.GetSettingValue("ScreenType");

        // 해상도 리스트 생
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.Reverse();

        // 드롭다운 해상도 입력
        List<string> options = new List<string>();
        int index =0;
        foreach (var resolution in resolutions)
        {
            string option = $"{resolution.width} x {resolution.height} {resolution.refreshRate}hz";
            if (option.Equals(nowSize)) {
                nowSizeIndex = index;
            }
            else {
                index++;
            }
            options.Add(option);
        }

        // 해상도 셋팅
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = nowSizeIndex;
      

        resolutionDropdown.RefreshShownValue();
    }

    private void SetScreenType() {

        List<string> options = new List<string>();
        options.Add(LocalizeManager.Instance.GetText("UI_ScreenType_Full"));
        options.Add(LocalizeManager.Instance.GetText("UI_ScreenType_FullWindow"));
        options.Add(LocalizeManager.Instance.GetText("UI_ScreenType_Window"));

       

        // 해상도 셋팅
        screenTypeDropDown.ClearOptions();
        screenTypeDropDown.AddOptions(options);
        screenTypeDropDown.value = SettingManager.Instance.GetSettingValue("ScreenType");
        screenTypeDropDown.RefreshShownValue();

    }
    public void ScreenTypeDropdownOptionChanged(bool ignoreOption = false) { 
        if(SettingManager.Instance.GetSettingValue("ScreenType") != screenTypeDropDown.value || ignoreOption) {
            SettingManager.Instance.SetSettingValue("ScreenType",screenTypeDropDown.value);
            switch (screenTypeDropDown.value) { 
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
    }
    // 해상도를 변경합니다
    public void DropdownOptionChanged(int resolutionIndex)
    {

        if (nowSizeIndex != resolutionDropdown.value) {
            nowSizeIndex = resolutionDropdown.value;
            Resolution resolution = resolutions[resolutionDropdown.value];
            Screen.SetResolution(resolution.width, resolution.height, true, resolution.refreshRate);
            ScreenTypeDropdownOptionChanged(true);
            SettingManager.Instance.SetSettingValue("ScreenWidth", resolution.width);
            SettingManager.Instance.SetSettingValue("ScreenHeight", resolution.height);
            SettingManager.Instance.SetSettingValue("ScreenRate", resolution.refreshRate);


        }
    }

    public void LangDropdownOptionChanged()
    { 
       
        if (langDropDown.value != SettingManager.Instance.GetSettingValue("lang")) {
            DebugManager.Instance.PrintDebug("[Setting] Set Lang to " + langDropDown.value);
            SettingManager.Instance.SetSettingValue("lang", langDropDown.value);
            LocalizeManager.Instance.SetLangType();
            foreach (UI_LocalizeText lt in textList) { 
                lt.ResetLang();
            }
            SetScreenType();

        }
        

    }

    // 창모드로 변경합니다
    public void SkipCutSceneToggleChanged()
    {
        SettingManager.Instance.SetSettingValue("CutScene",  skipCutScene.isOn?1:0 );
    }


    public void SetBootOption() {
            // 해상도 및 전체 화면 모드 저장
        PlayerPrefs.SetInt("ScreenWidth", SettingManager.Instance.GetSettingValue("ScreenWidth"));  
        PlayerPrefs.SetInt("ScreenHeight", SettingManager.Instance.GetSettingValue("ScreenHeight"));
        PlayerPrefs.SetInt("TargetFrameRate", SettingManager.Instance.GetSettingValue("ScreenRate"));
        switch (SettingManager.Instance.GetSettingValue("ScreenType")) { 
            case 0:
                PlayerPrefs.SetInt("FullScreenMode", 1);
                break;
                case 1:
                PlayerPrefs.SetInt("FullScreenMode", 1);
                PlayerPrefs.SetInt("BorderlessFullScreen", 1);
                break;
                case 2:
                PlayerPrefs.SetInt("FullScreenMode", 0);
                break;
            }
            
      

            // 변경 사항 저장
         PlayerPrefs.Save();
        

    }
}
