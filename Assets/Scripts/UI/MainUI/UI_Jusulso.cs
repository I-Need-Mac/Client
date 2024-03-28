using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UIStatus;

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
    List<UI_JusulsoPossesionSlot> slotList = new List<UI_JusulsoPossesionSlot>();
    [SerializeField]
    GameObject progressSlot;
    [SerializeField]
    public List<UI_JusulsoProgressBox> progressList = new List<UI_JusulsoProgressBox>();

    public UI_JusulsoReward reward;

    public DateTime currentTime;

    public string item1 = "box_1";
    public string item2 = "key";

    void Start()
    {
        RequestBox();
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
        for (int i = 0; i < 16; i++)
        {
            UI_JusulsoPossesionSlot possesionSlot = Util.UILoad<UI_JusulsoPossesionSlot>(Define.UiPrefabsPath + "/UI_JusulsoPossesionSlot");
            GameObject slotGameObject = Instantiate(possesionSlot.gameObject);
            UI_JusulsoPossesionSlot slotComponent = slotGameObject.GetComponent<UI_JusulsoPossesionSlot>();
            slotGameObject.transform.SetParent(slot_Page.transform);
            slotList.Add(slotComponent); // UI_JusulsoPossesionSlot 타입으로 캐스팅하여 추가
        }
        UI_JusulsoProgressBox progressBox = Util.UILoad<UI_JusulsoProgressBox>(Define.UiPrefabsPath + "/UI_JusulsoProgressBox");
        for(int i = 0; i<3; i++)
        {
            UI_JusulsoProgressBox slot = Instantiate(progressBox);
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

    async void RequestBox()
    {
        OwnBoxResult result = await APIManager.Instance.GetBox();
        currentTime = result.data.current_time;
        for (int i = 0; i < result.data.userRewardBoxes.Count; i++)
        {
            slotList[i].SetItem();
            slotList[i].SetItemData(result.data.userRewardBoxes[i]);
            if (slotList[i].box.open_start_time != null)
            {
                slotList[i].MoveItemToProgressBox();
            }
        }    
    }
}
