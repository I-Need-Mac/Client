using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        Sorcere,
    }

    [SerializeField]
    TextMeshProUGUI storyModeText;
    [SerializeField]
    TextMeshProUGUI selectSorcererText;
    [SerializeField]
    TextMeshProUGUI soulText;
    [SerializeField]
    TextMeshProUGUI sorcereText;

    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));

        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }

        storyModeText.text = LocalizeManager.Instance.GetText("UI_StoryMode");
        selectSorcererText.text = LocalizeManager.Instance.GetText("UI_SelectSorcerer");
        soulText.text = LocalizeManager.Instance.GetText("UI_Soul");
        sorcereText.text = LocalizeManager.Instance.GetText("UI_Sorcer");
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
                UIManager.Instance.OpenUI<UI_StoryMain>();
                break;
            case GameObjects.SelectSorcerer:
                UIManager.Instance.OpenUI<UI_SelectSorcerer>();
                break;
            case GameObjects.Soul:
                break;
            case GameObjects.Sorcere:
                break;
            default:
                break;
        }
    }
}
