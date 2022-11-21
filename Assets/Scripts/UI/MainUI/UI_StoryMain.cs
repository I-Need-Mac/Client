using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoryMain : UI_Popup
{
    enum Images
    {
        BackBtn,
        StartButton,
    }

    [SerializeField]
    Text title;
    [SerializeField]
    GameObject chapterList;
    [SerializeField]
    GameObject stageList;
    [SerializeField]
    GameObject chapterInfo;

    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        title.text = LocalizeManager.Instance.GetText("UI_StoryMode");
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
                this.CloseUI<UI_StoryMain>();
                break;
            case Images.StartButton:
                UI_StoryBook storyBook = UIManager.Instance.OpenUI<UI_StoryBook>();
                storyBook.SetData(UI_StoryBook.StoryBookID.STORY_BOOK_TITLE1);
                break;
            default:
                break;
        }
    }
}
