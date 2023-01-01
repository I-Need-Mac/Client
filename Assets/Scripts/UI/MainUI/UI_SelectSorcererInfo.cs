using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectSorcererInfo : UI_Popup
{
    enum Images
    {
        BackBtn
    }

    enum Buttons
    {
        StoryBtn,
        StatBtn,
        SkillBtn,
    }

    [SerializeField]
    GameObject StoryContents;
    [SerializeField]
    GameObject StatContents;
    [SerializeField]
    GameObject SkillContents;

    // Start is called before the first frame update
    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        Bind<Button>(typeof(Buttons));
        Array buttonValue = Enum.GetValues(typeof(Buttons));
        for (int i = 0; i < buttonValue.Length; i++)
        {
            BindUIEvent(GetButton(i).gameObject, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
        }

        StoryContents.gameObject.SetActive(true);
        StatContents.gameObject.SetActive(false);
        SkillContents.gameObject.SetActive(false);
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
                UIManager.Instance.CloseUI<UI_SelectSorcererInfo>();
                break;
            default:
                break;
        }
    }

    public void OnClickButton(PointerEventData data)
    {
        Buttons buttonValue = (Buttons)FindEnumValue<Buttons>(data.pointerClick.name);
        if ((int)buttonValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (buttonValue)
        {
            case Buttons.StoryBtn:
                StoryContents.gameObject.SetActive(true);
                StatContents.gameObject.SetActive(false);
                SkillContents.gameObject.SetActive(false);
                break;
            case Buttons.StatBtn:
                StoryContents.gameObject.SetActive(false);
                StatContents.gameObject.SetActive(true);
                SkillContents.gameObject.SetActive(false);
                break;
            case Buttons.SkillBtn:
                StoryContents.gameObject.SetActive(false);
                StatContents.gameObject.SetActive(false);
                SkillContents.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
