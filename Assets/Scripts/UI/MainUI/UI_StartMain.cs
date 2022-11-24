using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StartMain : UI_Base
{
    enum Images
    {
        Title,
        Version,
        PressKey,
    }

    float time = 0.0f;

    // ���� â
    Image pressKeyImage;

    void Start()
    {
        Bind<Image>(typeof(Images));

        Array imageValue = Enum.GetValues(typeof(Images));

        // ��ư �̺�Ʈ ���
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

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
        Color color = pressKeyImage.color;
        if (color.a >= 1f)
            time = Time.deltaTime * -1;
        else if (color.a <= 0f)
            time = Time.deltaTime;

        color.a += time;
        pressKeyImage.color = color;

        if (Input.anyKeyDown)
        {
            //Debug.Log("anykey");
        }
    }
}
