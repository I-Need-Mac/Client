using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameMain : UI_Popup
{
    enum GameObjects
    {
        StoryMode,
        SelectSorcerer,
        Soul,
        Sorcere
    }

    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));

        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
    }

    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case GameObjects.StoryMode:
                break;
            case GameObjects.SelectSorcerer:
                break;
            case GameObjects.Soul:
                break;
            case GameObjects.Sorcere:
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.OpenUI<UI_ESCPopup>();
            //Debug.Log("anykey");
        }
    }
}
