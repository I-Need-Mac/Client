using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using static UIStatus;

public class UI_JusulsoPossesionSlot : UI_Base
{
    public bool hasBox;
    public UI_Jusulso_Box box;
    public Sprite itemImage;
    public Image slotImage;
    private Button slotButton;
    public List<UI_JusulsoProgressBox> progressSlot;
    private UI_Jusulso jusulso;
    // Start is called before the first frame update
    void Start()
    {
        jusulso = FindObjectOfType<UI_Jusulso>();
        progressSlot = jusulso.progressList;
        for (int i = 0; i < progressSlot.Count; i++)
        {
            progressSlot[i] = jusulso.progressList[i];
        }

        slotButton = GetComponentInChildren<Button>();
        slotButton.onClick.AddListener(ClickSlot);
    }
    public void ClickSlot()
    {
        if (box == null)
        {
            DebugManager.Instance.PrintDebug("슬롯에 아이템 프리팹이 없습니다");
        }
        else
        {
            RequestBoxOpenStart();
            MoveItemToProgressBox();
        }
    }
    public void SetItem()
    {       
        box = Instantiate(ResourcesManager.Load<UI_Jusulso_Box>("Prefabs/InGame/Item/Item_Box"), transform);

        if (box.open_start_time==null)
        {
            slotImage.sprite = itemImage;
            hasBox = true;
        }

        SetImageAlpha();
    }
    public void SetItemData(OwnBoxResult.OwnBoxData data)
    {
        box.id= data.id;
        box.steam_id= data.steam_id;
        box.box_type= data.box_type;
        box.stage_id= data.stage_id;
        box.open_start_time= data.open_start_time;
        box.is_open= data.is_open;
        box.created_at= data.created_at;
        box.updated_at= data.updated_at;
    }
    public void MoveItemToProgressBox()
    {
        for (int i = 0; i < progressSlot.Count; i++)
        {
            if (!progressSlot[i].hasBox)
            {            
                progressSlot[i].ReceiveItem(box);
                box.transform.SetParent(progressSlot[i].transform);
                box = null;
                
                hasBox= false;
                SetImageAlpha();
                break;
            }
            else
            {
                DebugManager.Instance.PrintDebug("진행 슬롯이 다 차있습니다");
            }
        }

    }
    public void SetImageAlpha()
    {
        if (!hasBox)
        {
            slotImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            slotImage.color = new Color(1, 1, 1, 1);
        }
    }

    async void RequestBoxOpenStart()
    {
        if (box != null&&box.open_start_time == null)
        {
            BoxOpenStart boxOpen = await APIManager.Instance.BoxOpenStart(box.id);
            if(boxOpen.statusCode == 200)
            {
                DebugManager.Instance.PrintDebug("박스 열기 시작!");
            }
        }
        else if(box.open_start_time !=null)
        {
            DebugManager.Instance.PrintDebug("이미 진행중인 상자입니다");
        }
    }
}
