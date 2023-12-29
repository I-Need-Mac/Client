
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Sorcerer_Unlock_conditions : UI_Popup
{
    enum Images
    {
        Close,
        Click
    }

    [SerializeField] TextMeshProUGUI ment;
    [SerializeField] TextMeshProUGUI keyCount;
    [SerializeField] GameObject buyBtn;
    private void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
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
            case Images.Close:
                this.CloseUI<UI_Sorcerer_Unlock_conditions>();
                break;
            default:
                break;
        }
    }

    public void SetSorcererName(string name) { 
        ment.text = LocalizeManager.Instance.GetText("UI_BuySorcerer", UIStatus.Instance.sorcererCost, $"\"{LocalizeManager.Instance.GetText(name)}\"");
        keyCount.text = UIStatus.Instance.key.ToString();

        if (UIStatus.Instance.sorcererCost > UIStatus.Instance.key) { 
            keyCount.color = Color.red;
            buyBtn.GetComponent<SoundRequesterBtn>().SetIsNagative(false);
        }
    }

}
