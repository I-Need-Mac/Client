using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Hon : UI_Popup
{
    enum Images
    {
        Backbutton
    }
    enum GameObjects
    {
        Hon_Page1,
        Hon_Page2,
        Hon_Page3,
        Hon_Page4,
        Hon_Page5
    }

    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        DebugManager.Instance.PrintDebug(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.Backbutton:
                this.CloseUI<UI_Hon>();
                break;
            default:
                break;
        }
    }
    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;
        DebugManager.Instance.PrintDebug(data.pointerClick.name);

        UI_Hon_Under honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
        switch (imageValue)
        {
            case GameObjects.Hon_Page1:
                honUnder.Setting(101);
                break;
            case GameObjects.Hon_Page2:
                honUnder.Setting(102);
                break;
            case GameObjects.Hon_Page3:
                honUnder.Setting(103);
                break;
            case GameObjects.Hon_Page4:
                honUnder.Setting(104);
                break;
            case GameObjects.Hon_Page5:
                honUnder.Setting(105);
                break;


        }
    }
}
