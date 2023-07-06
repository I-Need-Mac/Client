using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_ChapterElement : UI_Base
{
    enum Images
    {
        Lock,
        Unlock,
    }

    enum TextMeshProGUIs
    {
        ChapterText
    }

    public TextMeshProUGUI text;
    public Image unlockImg;
    public Image lockImg;

    void Start()
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

    }
}
