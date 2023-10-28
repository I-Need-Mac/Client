using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Jusulso : UI_Popup
{
    enum GameObjects
    {
        Close
    }
    [SerializeField]
    TextMeshProUGUI Jusulso_title;
    [SerializeField]
    TextMeshProUGUI Progress_Box;
    [SerializeField]
    TextMeshProUGUI Possesion_Box;
    [SerializeField]
    TextMeshProUGUI Close_text;
    // Start is called before the first frame update
    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case GameObjects.Close:
                UIManager.Instance.CloseUI<UI_Jusulso>();
                break;
            default:
                break;
        }
    }
}
