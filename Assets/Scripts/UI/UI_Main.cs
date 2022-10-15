using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 메인화면UI
public class UI_Main : UI_Base
{
    enum Buttons
    {
        Inventory,          // 인벤토리
        Skill,              // 스킬
        StoryBook,          // 스토리북
    }

    void Start()
    {
        Bind<Button>(typeof(Buttons));

        Array buttonValue = Enum.GetValues(typeof(Buttons));

        // 버튼 이벤트 등록
        for (int i = 0; i < buttonValue.Length; i++)
        {
            BindUIEvent(GetButton(i).gameObject, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
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
            case Buttons.Inventory:
                UIManager.Instance.OpenUI<UI_Inventory>();
                break;
            case Buttons.Skill:
                UIManager.Instance.OpenUI<UI_Skill>();
                break;
            case Buttons.StoryBook:
                UI_StoryBook ui = UIManager.Instance.OpenUI<UI_StoryBook>();
                if (ui == null)
                    return;

                ui.SetData(UI_StoryBook.StoryBookID.STORY_BOOK_TITLE1);
                break;
            default:
                break;
        }
    }
}