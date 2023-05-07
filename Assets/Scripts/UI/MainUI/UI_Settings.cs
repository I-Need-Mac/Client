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

    [SerializeField] Toggle windowMode;
    [SerializeField] TMP_Dropdown resolutionDropdown;

    List<Resolution> resolutions;

    public int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex", 0);
        set => PlayerPrefs.SetInt("ResolutionIndex", value);
    }

    public bool IsWindowMode
    {
        get => PlayerPrefs.GetInt("IsWindowMode", 1) == 1;
        set => PlayerPrefs.SetInt("IsWindowMode", value ? 1 : 0);
    }


    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

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
                this.CloseUI<UI_Settings>();
                break;
            default:
                break;
        }
    }

    void SetResolution()
    {
        // 해상도 리스트 생
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.Reverse();

        // 드롭다운 해상도 입력
        List<string> options = new List<string>();
        foreach (var resolution in resolutions)
        {
            string option = $"{resolution.width} x {resolution.height}";
            options.Add(option);
        }

        // 해상도 셋팅
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = ResolutionIndex;
        windowMode.isOn = IsWindowMode;

        resolutionDropdown.RefreshShownValue();

        DropdownOptionChanged(ResolutionIndex);
    }

    // 해상도를 변경합니다
    public void DropdownOptionChanged(int resolutionIndex)
    {
        ResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    // 창모드로 변경합니다
    public void WindowModeToggleChanged(bool isWindow)
    {
        IsWindowMode = isWindow;
        Screen.fullScreen = !isWindow;
    }
}
