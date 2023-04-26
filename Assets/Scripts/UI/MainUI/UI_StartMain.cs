using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StartMain : UI_Base
{
    enum Images
    {
        Title,
        PressKey,
    }

    float time = 0.0f;

    // 시작 창
    Image pressKeyImage;

    [SerializeField]
    TextMeshProUGUI version;

    void Start()
    {
        Bind<Image>(typeof(Images));

        Array imageValue = Enum.GetValues(typeof(Images));

        // 버튼 이벤트 등록
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        version.text = "ver. 1.0.0";
        pressKeyImage = GetImage((int)Images.PressKey).gameObject.GetComponent<Image>();
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.PressKey:
                UIManager.Instance.OpenUI<UI_Login>();
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        if(pressKeyImage)
        {
            Color color = pressKeyImage.color;
            if (color.a >= 1f)
                time = Time.deltaTime * -1;
            else if (color.a <= 0f)
                time = Time.deltaTime;

            color.a += time;
            pressKeyImage.color = color;
        }

        OnPressKeyDown();
    }

    public void OnPressKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!UIManager.Instance.IsUiPopup("UI_ESCPopup"))
                UIManager.Instance.OpenUI<UI_ESCPopup>();
            else
                UIManager.Instance.CloseUI<UI_ESCPopup>();
        }
    }
}
