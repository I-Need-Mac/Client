using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoryBook : UI_Popup
{
    enum Buttons
    {
        PreArrow,
        NextArrow,

        PageSkip,
        AllSkip,

        Close
    }

    enum Images
    {
    }

    enum Texts
    {
    }

    public int page = 0;

    // Start is called before the first frame update
    void Start()
    {
        Bind<Button>(typeof(Buttons));
        Array buttonValue = Enum.GetValues(typeof(Buttons));

        // 버튼 이벤트 등록
        for(int i = 0; i < buttonValue.Length; i++)
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
            case Buttons.PreArrow:
                break;
            case Buttons.NextArrow:
                break;
            case Buttons.PageSkip:
                break;
            case Buttons.AllSkip:
                break;
            case Buttons.Close:
                CloseUI<UI_StoryBook>();
                break;
            default:
                break;
        }
    }
}
