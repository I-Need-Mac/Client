using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_SelectSorcerer : UI_Popup
{
    

    enum Images
    {
        BackBtn,
        Key
    }

    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    TextMeshProUGUI keyCountText;
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

        UIStatus.Instance.uI_SelectSorcerer = this;
        titleText.text = LocalizeManager.Instance.GetText("UI_SelectSorcerer");
        keyCountText.text = UIStatus.Instance.key.ToString();
        Dictionary<string, Dictionary<string, object>> characterData = UIData.CharacterData;
        UI_Sorcerer sorcerer = Util.UILoad<UI_Sorcerer>(Define.UiPrefabsPath + "/UI_Sorcerer");
        

        foreach (KeyValuePair<string, Dictionary<string, object>> data in characterData)
        {
            if (data.Key == "")
                continue;
            if (Convert.ToInt32(data.Key)>200)
                continue;
            // 생성
            GameObject instance = Instantiate(sorcerer.gameObject) as GameObject;
            instance.GetComponent<UI_Sorcerer>().SetSorcerer(int.Parse(data.Key), data.Value);
            
            instance.name = "sorcerer_" + data.Key;
            instance.transform.SetParent(sorcererObject.transform);
            instance.transform.localScale = Vector3.one;
            if (UIStatus.Instance.selectedChar == Convert.ToInt32(data.Key)) {
                instance.GetComponent<UI_Sorcerer>().SetIsSelected(true);
            }

            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;

            sorcererList.Add(instance);
        }

        DebugManager.Instance.PrintDebug("[SelectSorcerer] Selected Char " + UIStatus.Instance.selectedChar);
        // 위치 셋팅
        SetCharacterPos();
    }

    private void SetCharacterPos()
    {
        RectTransform r = sorcererList[0].GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(-750, 0, 0);
        r = sorcererList[1].GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(-450, 0, 0);
        r = sorcererList[2].GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(-150, 0, 0);
        r = sorcererList[3].GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(150, 0, 0);
        r = sorcererList[4].GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(450, 0, 0);
        r = sorcererList[5].GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(750, 0, 0);
    }

    public void SetCharacterState() { 
       keyCountText.text = UIStatus.Instance.key.ToString();
        foreach(GameObject s in sorcererList) {
            DebugManager.Instance.PrintDebug("[Sorcerer] Sorcerer Set ", s.GetComponent<UI_Sorcerer>().sorcererInfoID);
            s.GetComponent<UI_Sorcerer>().CharacterUnlock();
            s.GetComponent<UI_Sorcerer>().SetIsSelected();

        }
    }
    public void AllLock()
    {

        foreach (GameObject s in sorcererList)
        {
            DebugManager.Instance.PrintDebug("[Sorcerer] Sorcerer Set ", s.GetComponent<UI_Sorcerer>().sorcererInfoID);
            s.GetComponent<UI_Sorcerer>().SetIsLocked(false);

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
            case Images.BackBtn:
                UIManager.Instance.CloseUI<UI_SelectSorcerer>();
                break;
            case Images.Key:
                AllLock();
                break;
            default:
                break;
        }
    }
}
