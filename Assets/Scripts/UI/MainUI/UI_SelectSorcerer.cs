using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectSorcerer : UI_Popup
{
    enum TableIndex
    {
        Character_Name,
        Image_Path
    }

    enum Images
    {
        Sorcerer_0,
        Sorcerer_1,
        Sorcerer_2,
        Sorcerer_3,
        Sorcerer_4,
        Sorcerer_5,

        BackBtn
    }

    [SerializeField]
    Text titleText;
    [SerializeField]
    List<GameObject> characterList = new List<GameObject>();

    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        titleText.text = LocalizeManager.Instance.GetText("UI_SelectSorcerer");

        //Dictionary<string, Dictionary<string, object>> characterList = UIData.CharacterData;
        //foreach (KeyValuePair<string, Dictionary<string, object>> data in characterList)
        //{
        //    foreach( KeyValuePair<string, object> val in data.Value )
        //    {

        //    }
        //}
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.BackBtn:
                UIManager.Instance.CloseUI<UI_SelectSorcerer>();
                break;
            case Images.Sorcerer_0:
                UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                break;
            case Images.Sorcerer_1:
                UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                break;
            case Images.Sorcerer_2:
                UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                break;
            case Images.Sorcerer_3:
                UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                break;
            case Images.Sorcerer_4:
                UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                break;
            case Images.Sorcerer_5:
                UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                break;
            default:
                break;
        }
    }
}
