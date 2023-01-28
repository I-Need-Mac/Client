using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    Text titleText;
    [SerializeField]
    Text loginText;
    [SerializeField]
    Text autoLoginText;

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
        }

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
                WebLoginFromPost();
                break;
            default:
                break;
        }
    }

    async void WebLoginFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "mongplee92");

        var data = await WebRequestManager.Instance.Post<Dictionary<string, object>>("/user/login", sendData);

        //switch(data.result)
        //{
        //    case 100:
        //        // result 100 : 로그인 성공
        //        break;
        //    case 200:
        //        // result 200 : DB에 없는 유저(회원가입 진행)
                //this.CloseUI<UI_Login>();
                //UIManager.Instance.OpenUI<UI_Agreement>();
        //        break;
        //}
    }
}
