using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Popup
{
    enum Images
    {
        LoginButton,
    }

    [SerializeField]
    Sprite loginButtonOrigin;
    [SerializeField]
    Sprite loginButtonHover;

    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    TextMeshProUGUI loginText;
    [SerializeField]
    TextMeshProUGUI autoLoginText;

    Toggle autoLogin;

    // Start is called before the first frame update
    void Start()
    {
        Bind<Image>(typeof(Images));

        Array imageValue = Enum.GetValues(typeof(Images));

        // 버튼 이벤트 등록
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnEnterImage(data); }, Define.UIEvent.Enter);
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnExitImage(data); }, Define.UIEvent.Exit);
        }

        //loginButtonOrigin = GetImage((int)Images.LoginButton).gameObject.GetComponent<Image>();

        titleText.text = LocalizeManager.Instance.GetText("UI_Login");
        loginText.text = LocalizeManager.Instance.GetText("UI_LoginWithSteam");
        autoLoginText.text = LocalizeManager.Instance.GetText("UI_AutoLogin");
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.LoginButton:
                // 로그인 처리
                // WebLoginFromPost();
                this.CloseUI<UI_Login>();
                UIManager.Instance.OpenUI<UI_NickName>();
                if (!SteamManager.Initialized) { return; }

                break;
            default:
                break;
        }
    }

    public void OnEnterImage(PointerEventData data)
    {
        if(loginButtonHover != null)
        {
            GetImage((int)Images.LoginButton).gameObject.GetComponent<Image>().sprite = loginButtonHover;
        }
    }

    public void OnExitImage(PointerEventData data)
    {
        if (loginButtonOrigin != null)
        {
            GetImage((int)Images.LoginButton).gameObject.GetComponent<Image>().sprite = loginButtonOrigin;
        }
    }


}
