using System;
using System.Collections;
using System.Collections.Generic;
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
        Setting
    }

    [SerializeField]
    Text myInfoText;
    [SerializeField]
    Text rankingText;
    [SerializeField]
    Text friendText;
    [SerializeField]
    Text settingText;

    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));

        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
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
                this.CloseUI<UI_ESCPopup>();
                UIManager.Instance.OpenUI<UI_MyInfo>();
                break;
            case GameObjects.RankBoard:
                break;
            case GameObjects.Friend:
                break;
            case GameObjects.Setting:
                break;
            default:
                break;
        }
    }
}