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
        Sorcere,
        StoryBook
    }

    [SerializeField]
    Text storyModeText;
    [SerializeField]
    Text selectSorcererText;
    [SerializeField]
    Text soulText;
    [SerializeField]
    Text sorcereText;

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
            case GameObjects.StoryBook:
                UI_StoryBook storyBook = UIManager.Instance.OpenUI<UI_StoryBook>();
                storyBook.SetData(UI_StoryBook.StoryBookID.STORY_BOOK_TITLE1);
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
