using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class UI_Hon_Under : UI_Popup
{
    private Dictionary<string, Dictionary<string, object>> soulTable;
    private int[] soulIds;
    private int seonghonId;

    enum Images
    {
        Hon_MainBox,
        Back,
        Save,
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
        soulIds = new int[18];

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
        //TestFunction();
        this.soulTable = CSVReader.Read("UnderSoul");
        this.seonghonId = mainCategoryId;

        GetImage(0).sprite = ResourcesManager.Load<Sprite>("Arts/" + CSVReader.Read("MainCategorySoul", mainCategoryId.ToString(), "SoulMainImagePath").ToString());
        GetImage(0).GetComponentInChildren<TMP_Text>().text = LocalizeManager.Instance.GetText(CSVReader.Read("MainCategorySoul", mainCategoryId.ToString(), "SoulMainNameText").ToString());

        foreach (string id in  soulTable.Keys)
        {
            try
            {
                if (mainCategoryId == Convert.ToInt32(soulTable[id]["SoulMainCategory"]))
                {
                    int num = 3 * (Convert.ToInt32(soulTable[id]["SoulColumnGroup"]) - 1) + Convert.ToInt32(soulTable[id]["SoulOrderInColumn"]) - 1;
                    DebugManager.Instance.PrintError("ID: {0}, Num: {1}", id, num);
                    GameObject underSoul = GetGameObject(num);
                    SetSoulIconSet(underSoul, id);
                    soulIds[num] = Convert.ToInt32(id);
                }
            }
            catch (KeyNotFoundException e)
            {
                DebugManager.Instance.PrintError("[UI_Hon_Under: Error] UnderSoul 테이블에 빈 줄이 삽입되어 있습니다.");
            }
        }

        UnderSoulInit();
    }

    public async void TestFunction()
    {
        SoulProgress t = new SoulProgress();
        NormalResult nr = await APIManager.Instance.SoulProgressUpdate(seonghonId, t);
        DebugManager.Instance.PrintError(nr.message);
    }

    public async void SetSoulIconSet(GameObject obj, string id)
    {
        obj.GetComponent<Image>().sprite = ResourcesManager.Load<Sprite>("Arts/Hon/" + soulTable[id]["SoulImagePath"].ToString());
        obj.GetComponentInChildren<TMP_Text>().text = LocalizeManager.Instance.GetText(soulTable[id]["SoulNameText"].ToString());
        //언락체크
        if (Enum.TryParse(soulTable[id]["SoulUnlock"].ToString(), true, out SOUL_UNLOCK unlock))
        {
            //List<string> list = soulTable[id]["UnlockParam"] as List<string>;
            //if (list == null)
            //{
            //    if (int.TryParse(soulTable[id]["UnlockParam"].ToString(), out int result))
            //    {
            //        int count = AchievementManager.Instance.GetSoulUnlockCount(unlock, new List<int>() { result });
            //        obj.transform.Find("Lock").GetComponent<Image>().enabled = !await APIManager.Instance.UnlockSoul(seonghonId, int.Parse(id), count);
            //    }
            //    else
            //    {
            //        int count = AchievementManager.Instance.GetSoulUnlockCount(unlock, new List<int>());
            //        obj.transform.Find("Lock").GetComponent<Image>().enabled = !await APIManager.Instance.UnlockSoul(seonghonId, int.Parse(id), count);
            //    }
            //}
            //else
            //{
            //    List<int> list2 = new List<int>();
            //    foreach (string s in list)
            //    {
            //        if (int.TryParse(s, out int result))
            //        {
            //            list2.Add(result);
            //        }
            //    }
            //    int count = AchievementManager.Instance.GetSoulUnlockCount(unlock, list2);
            //    obj.transform.Find("Lock").GetComponent<Image>().enabled = !await APIManager.Instance.UnlockSoul(seonghonId, int.Parse(id), count);
            //}
            if (int.TryParse(soulTable[id]["Count"].ToString(), out int count))
            {
                obj.transform.Find("Lock").GetComponent<Image>().enabled = !await APIManager.Instance.UnlockSoul(seonghonId, int.Parse(id), count);
            }
        }
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
            case Images.Back:
                this.CloseUI<UI_Hon_Under>();
                break;
            case Images.Save:
                this.CloseUI<UI_Hon_Under>();
                Save();
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

        //우클릭일 경우
        if (data.button == PointerEventData.InputButton.Right)
        {
        }
        else
        {
            if (data.pointerClick.transform.Find("Lock").GetComponent<Image>().enabled)
            {
                return;
            }

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

    private void UnderSoulInit()
    {
        foreach (int id in SoulManager.Instance.GetSoulIdList(seonghonId))
        {
            for (int i = 0; i < soulIds.Length; i++)
            {
                if (soulIds[i] == id)
                {
                    GetGameObject(i).transform.Find("Shadow").GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    public void Save()
    {
        SoulManager.Instance.SeonghonReset(seonghonId);
        for (int i = 0; i < Enum.GetValues(typeof(GameObjects)).Length; i++)
        {
            if (GetGameObject(i).transform.Find("Shadow").GetComponent<Image>().enabled == false)
            {
                SoulManager.Instance.Add(seonghonId, new Soul(soulIds[i]));
            }
        }

        SoulManager.Instance.PrintSoulList(seonghonId);
    }
}
