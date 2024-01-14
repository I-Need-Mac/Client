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
    TextMeshProUGUI jusulso_Title;
    [SerializeField]
    TextMeshProUGUI progress_Box;
    [SerializeField]
    TextMeshProUGUI possesion_Box;
    [SerializeField]
    TextMeshProUGUI close_Text;

    [SerializeField]
    GameObject slot_Page;
    [SerializeField]
    List<GameObject> slotList = new List<GameObject>();
    [SerializeField]
    GameObject progressSlot;
    [SerializeField]
    List<GameObject> progressList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        jusulso_Title.text = LocalizeManager.Instance.GetText("UI_Sorcere_Title");
        progress_Box.text = LocalizeManager.Instance.GetText("UI_Sorcere_MyBoxes");
        possesion_Box.text = LocalizeManager.Instance.GetText("UI_Sorcere_AllBox");
        close_Text.text = LocalizeManager.Instance.GetText("UI_Result_Exit");
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
        UI_JusulsoPossesionSlot possesionSlot = Util.UILoad<UI_JusulsoPossesionSlot>(Define.UiPrefabsPath + "/UI_JusulsoPossesionSlot");
        for (int i = 0; i < 16; i++)
        {
            GameObject slot = Instantiate(possesionSlot.gameObject);
            slot.GetComponent<UI_JusulsoPossesionSlot>();
            slot.transform.SetParent(slot_Page.transform);
            slotList.Add(slot);
        }
        UI_JusulsoProgressBox progressBox = Util.UILoad<UI_JusulsoProgressBox>(Define.UiPrefabsPath + "/UI_JusulsoProgressBox");
        for(int i = 0; i<3; i++)
        {
            GameObject slot = Instantiate(progressBox.gameObject);
            slot.GetComponent<UI_JusulsoProgressBox>();
            slot.transform.SetParent(progressSlot.transform);
            progressList.Add(slot);
        }
        SetSlotPos();
        SetProgressSlotPos();
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
    private void SetSlotPos()
    {
        int rowSize = 8;
        float horizontalSpacing = 154;
        float verticalSpacing = 140;

        for (int i = 0; i < slotList.Count; i++)
        {
            RectTransform r = slotList[i].GetComponent<RectTransform>();
            int row = i / rowSize;
            int col = i % rowSize;

            float xPos = -540 + col * horizontalSpacing;
            float yPos = -150 - row * verticalSpacing;

            r.anchoredPosition3D = new Vector3(xPos, yPos, 0);
        }
    }
    private void SetProgressSlotPos()
    {
        float spacing = 392;
        for (int i = 0; i < progressList.Count; i++)
        {
            RectTransform r = progressList[i].GetComponent<RectTransform>();
            float pos = -392 + i * spacing;
            r.anchoredPosition3D = new Vector3(pos, 178, 0);
        }
    }

    

}
