using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill : UI_Popup
{
    enum Buttons
    {
        Close,
    }

    void Start()
    {
        Bind<Button>(typeof(Buttons));

        GameObject close = GetButton((int)Buttons.Close).gameObject;
        BindUIEvent(close, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
    }

    public void OnClickButton(PointerEventData data)
    {
        Buttons buttonValue = (Buttons)FindEnumValue<Buttons>(data.pointerClick.name);
        if ((int)buttonValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (buttonValue)
        {
            case Buttons.Close:
                CloseUI<UI_Skill>();
                break;
            default:
                break;
        }
    }
}
