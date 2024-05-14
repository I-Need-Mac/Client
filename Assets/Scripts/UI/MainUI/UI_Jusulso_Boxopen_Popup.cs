using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Jusulso_Boxopen_Popup : UI_Popup
{
    enum Images
    {
        Close
    }
    enum GameObjects
    {
        Yes,
        No
    }
    private UI_Jusulso jusulso;
    public UI_JusulsoProgressBox jusulsoProgressBox;
    void Start()
    {
        jusulso = FindObjectOfType<UI_Jusulso>();
        transform.SetParent(jusulso.transform);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        Vector3 position = rectTransform.localPosition;
        position.z = 0f; // z 축 값을 0으로 설정합니다.
        rectTransform.localPosition = position;
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
    }
    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case GameObjects.Yes:
                jusulsoProgressBox.RequestBoxOpen();
                Destroy(gameObject);
                break;
            case GameObjects.No:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
