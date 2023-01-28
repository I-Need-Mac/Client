using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectSorcerer : UI_Popup
{
    enum TableIndex
    {
        Character_Name,
        Image_Path
    }

    enum Images
    {
        BackBtn
    }

    [SerializeField]
    Text titleText;
    [SerializeField]
    GameObject sorcererObject;
    [SerializeField]
    List<GameObject> sorcererList = new List<GameObject>();

    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        titleText.text = LocalizeManager.Instance.GetText("UI_SelectSorcerer");

        UI_Sorcerer sorcerer = Util.UILoad<UI_Sorcerer>(Define.UiPrefabsPath + "/UI_Sorcerer");

        Dictionary<string, Dictionary<string, object>> characterList = UIData.CharacterData;
        foreach (KeyValuePair<string, Dictionary<string, object>> data in characterList)
        {
            if (data.Key == "")
                continue;

            GameObject instance = Instantiate(sorcerer.gameObject) as GameObject;
            instance.name = "sorcerer_" + data.Key;
            instance.transform.SetParent(sorcererObject.transform);
            instance.transform.localScale = Vector3.one;

            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;

            sorcererList.Add(instance);


            foreach (KeyValuePair<string, object> val in data.Value)
            {

            }
        }

        SetPositionCharacterImage();
    }

    public void SetPositionCharacterImage()
    {
        // 1개 = 0
        // 2개
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
                UIManager.Instance.CloseUI<UI_SelectSorcerer>();
                break;
            default:
                break;
        }
    }
}
