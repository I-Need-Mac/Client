using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ESCPopup : UI_Popup
{
    enum GameObjects
    {
        MyInfo,
        RankBoard,
        Friend,
        Setting,
        GameQuit,
    }

    enum Images
    {
        Close,
    }

    [SerializeField]
    TextMeshProUGUI myInfoText;
    [SerializeField]
    TextMeshProUGUI rankingText;
    [SerializeField]
    TextMeshProUGUI friendText;
    [SerializeField]
    TextMeshProUGUI settingText; 

    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }

        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        myInfoText.text = LocalizeManager.Instance.GetText("UI_MyInfo");
        rankingText.text = LocalizeManager.Instance.GetText("UI_RankingBoard");
        friendText.text = LocalizeManager.Instance.GetText("UI_Friend");
        settingText.text = LocalizeManager.Instance.GetText("UI_Setting");
    }

    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case GameObjects.MyInfo:
                UIManager.Instance.OpenUI<UI_MyInfo>();
                break;
            case GameObjects.RankBoard:
                UIManager.Instance.OpenUI<UI_RankBoard>();
                break;
            case GameObjects.Friend:
                break;
            case GameObjects.Setting:
                UIManager.Instance.OpenUI<UI_Settings>();
                break;
            case GameObjects.GameQuit:
                Application.Quit();
                break;
            default:
                break;
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
            case Images.Close:
                this.CloseUI<UI_ESCPopup>();
                break;
            default:
                break;
        }
    }
}
