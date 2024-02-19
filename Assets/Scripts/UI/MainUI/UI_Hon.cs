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

    // Start is called before the first frame update
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

        Debug.Log(data.pointerClick.name);

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
        Debug.Log(data.pointerClick.name);

        UI_Hon_Under honUnder;
        switch (imageValue)
        {
            case GameObjects.Hon_Page1:
                //UI_Honpage honPage1 = GetGameObject(0).GetComponent<UI_Honpage>();

                //if (honPage1 != null && honPage1.unlocked)
                //{
                //    honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                //    honUnder.Setting(101);
                //    Debug.Log("Hon_Page1 is Unlocked");
                //}
                //else
                //{
                //    UIManager.Instance.OpenUI<UI_Hon_Unlock_conditions>();
                //    Debug.Log("Hon_Page1 is locked");
                //}
                honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                honUnder.Setting(101);
                break;
            case GameObjects.Hon_Page2:
                //UI_Honpage honPage2 = GetGameObject(1).GetComponent<UI_Honpage>();
                //if (honPage2 != null && honPage2.unlocked)
                //{
                //    honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                //    honUnder.Setting(102);
                //    Debug.Log("Hon_Page2 is Unlocked");
                //}
                //else
                //{
                //    UIManager.Instance.OpenUI<UI_Hon_Unlock_conditions>();
                //    Debug.Log("Hon_Page2 is locked");
                //}
                honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                honUnder.Setting(102);
                break;
            case GameObjects.Hon_Page3:
                //UI_Honpage honPage3 = GetGameObject(2).GetComponent<UI_Honpage>();
                //if (honPage3 != null && honPage3.unlocked)
                //{
                //    honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                //    honUnder.Setting(103);
                //    Debug.Log("Hon_Page3 is Unlocked");
                //}
                //else
                //{
                //    UIManager.Instance.OpenUI<UI_Hon_Unlock_conditions>();
                //    Debug.Log("Hon_Page3 is locked");
                //}
                honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                honUnder.Setting(103);
                break;
            case GameObjects.Hon_Page4:
                //UI_Honpage honPage4 = GetGameObject(3).GetComponent<UI_Honpage>();
                //if (honPage4 != null && honPage4.unlocked)
                //{
                //    honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                //    honUnder.Setting(104);
                //    Debug.Log("Hon_Page4 is Unlocked");
                //}
                //else
                //{
                //    UIManager.Instance.OpenUI<UI_Hon_Unlock_conditions>();
                //    Debug.Log("Hon_Page4 is locked");

                //}
                honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                honUnder.Setting(104);
                break;
            case GameObjects.Hon_Page5:
                //UI_Honpage honPage5 = GetGameObject(4).GetComponent<UI_Honpage>();
                //if (honPage5 != null && honPage5.unlocked)
                //{
                //    honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                //    honUnder.Setting(105);
                //    Debug.Log("Hon_Page5 is Unlocked");
                //}
                //else
                //{
                //    UIManager.Instance.OpenUI<UI_Hon_Unlock_conditions>();
                //    Debug.Log("Hon_Page5 is locked");
                //}
                honUnder = UIManager.Instance.OpenUI<UI_Hon_Under>();
                honUnder.Setting(105);
                break;


        }
    }
}
