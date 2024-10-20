using Steamworks;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StartMain : UI_Base
{
    enum Images
    {
        Title,
        PressKey,
        TouchPanel
    }

    float time = 0.0f;
    private bool isFirst;
    private bool isAutoLogin;

    // 시작 창
    Image pressKeyImage;

    [SerializeField]
    TextMeshProUGUI version;
    [SerializeField]
    SoundRequesterBtn soundRequester;

    private bool isLogin;

    void Start()
    {
        Bind<Image>(typeof(Images));

        Array imageValue = Enum.GetValues(typeof(Images));

        isFirst = Convert.ToBoolean( SettingManager.Instance.GetSettingValue("FirstRegist"));
        isAutoLogin = Convert.ToBoolean(SettingManager.Instance.GetSettingValue("AutoLogin"));


        // 버튼 이벤트 등록
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        version.text = "ver. 1.0.0";
        pressKeyImage = GetImage((int)Images.PressKey).gameObject.GetComponent<Image>();
        SoundManager.Instance.RefindBGMRequester();
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
              case Images.TouchPanel:
                    if (isFirst)
                {
                    UIManager.Instance.OpenUI<UI_Login>();
                }
                else
                {
                    if (isAutoLogin)
                    {
                        //soundRequester.ChangeSituation(SoundSituation.SOUNDSITUATION.PRESS);
                        RequestLogin();

                    }
                    else
                    {
                        UIManager.Instance.OpenUI<UI_Login>();
                    }
                }
            //UIManager.Instance.OpenUI<UI_StoryMain>();
            break;

            default:
                break;
        }
    }

    public void Update()
    {
        if(pressKeyImage)
        {
            Color color = pressKeyImage.color;
            if (color.a >= 1f)
                time = Time.deltaTime * -1;
            else if (color.a <= 0f)
                time = Time.deltaTime;

            color.a += time;
            pressKeyImage.color = color;
        }

        OnPressKeyDown();
    }
     
    public void OnPressKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!UIManager.Instance.IsUiPopup("UI_ESCPopup"))
                UIManager.Instance.OpenUI<UI_ESCPopup>();
            else
                UIManager.Instance.CloseUI<UI_ESCPopup>();
        }
        else if(Input.anyKeyDown){

        }
   
    }

    async void RequestLogin()
    {
        if (!SteamManager.Initialized) { return; }
        string name = UIStatus.Instance.steam_id;
        isLogin = await APIManager.Instance.TryLogin(name);
        DebugManager.Instance.PrintDebug("[SteamAPI] load steam id "+name);
        if (isLogin)
        {
            UIManager.Instance.OpenUI<UI_GameMain>();
           
        }
        else
        {
            UIManager.Instance.OpenUI<UI_Login>();
        }
    }


}
