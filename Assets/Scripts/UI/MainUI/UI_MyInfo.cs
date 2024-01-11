using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MyInfo : UI_Popup
{
    enum Images
    {
        Close,
    }

    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    TextMeshProUGUI nickNameText;
    [SerializeField]
    TextMeshProUGUI customerCenterText;
    [SerializeField]
    TextMeshProUGUI emailText;
    [SerializeField]
    TextMeshProUGUI openKakaoText;

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

        titleText.text = LocalizeManager.Instance.GetText("UI_MyInfo");
        nickNameText.text = "닉네임";
        customerCenterText.text = LocalizeManager.Instance.GetText("UI_CustomerCenter");
        emailText.text = LocalizeManager.Instance.GetText("UI_Email");
        openKakaoText.text = LocalizeManager.Instance.GetText("UI_OpenKakaoTalk");
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
                this.CloseUI<UI_MyInfo>();
                break;
            default:
                break;
        }
    }
}
