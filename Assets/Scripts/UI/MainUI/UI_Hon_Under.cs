using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Hon_Under : UI_Popup
{
    enum Images
    {
        Hon_MainBox,
        Backbutton,
    }

    enum GameObjects
    {
        UnderSoul_1,
        UnderSoul_2,
        UnderSoul_3,
        UnderSoul_4,
        UnderSoul_5,
        UnderSoul_6,
        UnderSoul_7,
        UnderSoul_8,
        UnderSoul_9,
        UnderSoul_10,
        UnderSoul_11,
        UnderSoul_12,
        UnderSoul_13,
        UnderSoul_14,
        UnderSoul_15,
        UnderSoul_16,
        UnderSoul_17,
        UnderSoul_18,
    }

    private void Awake()
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
            BindUIEvent(GetGameObject(i), (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }

    }

    public void Setting(int mainCategoryId)
    {
        Dictionary<string, Dictionary<string, object>> table = CSVReader.Read("UnderSoul");

        foreach (string id in  table.Keys)
        {
            try
            {
                if (mainCategoryId == Convert.ToInt32(table[id]["SoulMainCategory"]))
                {
                    GameObject underSoul = GetGameObject(3 * (Convert.ToInt32(table[id]["SoulColumnGroup"]) - 1) + Convert.ToInt32(table[id]["SoulOrderInColumn"]) - 1);
                    SetSoulIconSet(underSoul, id);
                }
            }
            catch (KeyNotFoundException e)
            {
                DebugManager.Instance.PrintError("[UI_Hon_Under: Error] UnderSoul 테이블에 빈 줄이 삽입되어 있습니다.");
            }
        }
    }

    public void SetSoulIconSet(GameObject obj, string id)
    {
        obj.GetComponent<Image>().sprite = ResourcesManager.Load<Sprite>("Arts/Hon/" + CSVReader.Read("UnderSoul", id, "SoulImagePath").ToString());
        obj.GetComponentInChildren<TMP_Text>().text = LocalizeManager.Instance.GetText(CSVReader.Read("UnderSoul", id, "SoulNameText").ToString());

        //언락체크
        obj.transform.Find("Lock").GetComponent<Image>().enabled = false;
    }

    public void IsSelected(GameObject obj, bool isSelected)
    {
        obj.transform.Find("Shadow").GetComponent<Image>().enabled = !isSelected;
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
        {
            return;
        }

        DebugManager.Instance.PrintDebug(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.Backbutton:
                this.CloseUI<UI_Hon_Under>();
                break;
            default:
                break;
        }
    }

    public void OnClickObject(PointerEventData data)
    {
        int soulNum = FindEnumValue<GameObjects>(data.pointerClick.name);
        if (soulNum < -1)
        {
            return;
        }

        string targetName = data.pointerClick.name;
        //우클릭일 경우
        if (data.button == PointerEventData.InputButton.Right)
        {
        }
        else
        {
            switch(soulNum % 3)
            {
                case 0:
                    IsSelected(GetGameObject(soulNum), true);
                    IsSelected(GetGameObject(soulNum + 1), false);
                    IsSelected(GetGameObject(soulNum + 2), false);
                    break;
                case 1:
                    IsSelected(GetGameObject(soulNum - 1), false);
                    IsSelected(GetGameObject(soulNum), true);
                    IsSelected(GetGameObject(soulNum + 1), false);
                    break;
                case 2:
                    IsSelected(GetGameObject(soulNum - 2), false);
                    IsSelected(GetGameObject(soulNum - 1), false);
                    IsSelected(GetGameObject(soulNum), true);
                    break;
                default:
                    break;
            }
        }

    }
}
