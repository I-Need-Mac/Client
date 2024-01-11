using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Agreement : UI_Popup
{
    enum Images
    {
        AgreementBtn,
    }

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    TextMeshProUGUI checkTextTop;
    [SerializeField]
    TextMeshProUGUI checkTextBottom;

    [SerializeField]
    TextMeshProUGUI contentTextTop;
    [SerializeField]
    TextMeshProUGUI contentTextBottom;

    Image agreeBtn;

    [SerializeField]
    Toggle toggleTop;
    [SerializeField]
    Toggle toggleBottom;

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

        agreeBtn = GetImage((int)Images.AgreementBtn);
        agreeBtn.color = Color.gray;
        BindUIEvent(agreeBtn.gameObject, (PointerEventData data) => { OnEnterImage(data); }, Define.UIEvent.Enter);
        BindUIEvent(agreeBtn.gameObject, (PointerEventData data) => { OnExitImage(data); }, Define.UIEvent.Exit);

        titleText.text = LocalizeManager.Instance.GetText("UI_Agreement");
        checkTextTop.text = LocalizeManager.Instance.GetText("UI_AgreeCheck");
        checkTextBottom.text = LocalizeManager.Instance.GetText("UI_AgreeCheck");

        contentTextTop.text = LocalizeManager.Instance.GetText("UI_AutoLogin");
        contentTextBottom.text = LocalizeManager.Instance.GetText("UI_AutoLogin");

        toggleTop.isOn = false;
        toggleBottom.isOn = false;
        toggleTop.onValueChanged.AddListener(CheckToggle);
        toggleBottom.onValueChanged.AddListener(CheckToggle);
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.AgreementBtn:
                if (!toggleTop.isOn || !toggleBottom.isOn)
                {
                    return;
                }

                this.CloseUI<UI_Agreement>();
                UIManager.Instance.OpenUI<UI_GameMain>();
                break;
            default:
                break;
        }
    }

    public void OnEnterImage(PointerEventData data)
    {
        Debug.Log("OnEnterImage");
    }

    public void OnExitImage(PointerEventData data)
    {
        Debug.Log("OnExitImage");
    }

    void CheckToggle(bool toggle)
    {
        // 둘 다 동의해야합니다.
        if( toggleTop.isOn && toggleBottom.isOn )
        {
            agreeBtn.color = Color.white;
        }
        else
        {
            agreeBtn.color = Color.gray;
        }
    }
}
