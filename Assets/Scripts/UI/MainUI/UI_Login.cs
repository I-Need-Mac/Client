using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Popup
{
    // 스팀 로그인
    Image steamLogin;

    enum Images
    {
        LoginButton,
    }

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
                break;
            default:
                break;
        }
    }
}
