using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NickName : UI_Popup
{
    enum Images
    {
        Confirm,
    }

    [SerializeField]
    Text titleText;
    [SerializeField]
    Text noticeText;
    [SerializeField]
    Text ableText;
    [SerializeField]
    Text confirmText;

    Image confirm;

    [SerializeField]
    InputField inputText;
    Text nickname;

    bool isCreate;

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

        confirm = GetImage((int)Images.Confirm);
        confirm.color = Color.gray;

        titleText.text = LocalizeManager.Instance.GetText("UI_MakeNickname");
        Debug.Log(titleText.text);
        noticeText.text = LocalizeManager.Instance.GetText("Nickname_Able").Replace("\"", "");

        confirmText.text = "확인";


        ableText.text = "";

        inputText.onValueChanged.AddListener(CheckNickNameAble);
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.Confirm:
                if (!isCreate)
                    return;

                // 회원가입 진행
                WebHandShakeFromPost();

                this.CloseUI<UI_NickName>();
                UIManager.Instance.OpenUI<UI_Agreement>();
                break;
            default:
                break;
        }
    }

    public void CheckNickNameAble(string text)
    {
        isCreate = false;

        // 닉네임이 2자 미만 입니다.
        if (text.Length < 2 )
        {
            ableText.text = LocalizeManager.Instance.GetText("Nickname_Less");
            ableText.color = Color.red;
            confirm.color = Color.gray;
            return;
        }
        
        // 닉네임이 10자 초과 입니다.
        if (text.Length > 10)
        {
            ableText.text = LocalizeManager.Instance.GetText("Nickname_Lot");
            ableText.color = Color.red;
            confirm.color = Color.gray;
            return;
        }
        
        // 사용가능한 닉네임 입니다.
        ableText.text = LocalizeManager.Instance.GetText("Nickname_Able");
        ableText.color = Color.green;
        confirm.color = Color.white;
        isCreate = true;
    }

    async void WebHandShakeFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "mongplee92");
        sendData.Add("nick_name", "mongplee92");
        sendData.Add("admin_level", "0");

        var data = await WebRequestManager.Instance.Post<Dictionary<string, object>>("/user/handshake", sendData);

        //switch(data.result)
        //{
        //    case 100:
        //        // result 100 : 회원가입 성공
        //        break;
        //    case 200:
        //        // result 200 : 중복된 닉네임
        //        break;
        //}
    }
}
