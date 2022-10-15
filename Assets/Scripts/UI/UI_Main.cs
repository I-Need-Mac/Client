using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ����ȭ��UI
public class UI_Main : UI_Base
{
    enum Buttons
    {
        Inventory,          // �κ��丮
        Skill,              // ��ų
        StoryBook,          // ���丮��
    }

    void Start()
    {
        Bind<Button>(typeof(Buttons));

        Array buttonValue = Enum.GetValues(typeof(Buttons));

        // ��ư �̺�Ʈ ���
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