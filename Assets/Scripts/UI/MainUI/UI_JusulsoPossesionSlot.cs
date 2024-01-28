using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UI_JusulsoPossesionSlot : UI_Base
{
    public bool hasBox;
    public Item item;
    public Sprite itemImage;
    public Image slotImage;
    
    private Button slotButton;
    public UI_JusulsoProgressBox[] progressSlot;
    private UI_Jusulso jusulso;

    // Start is called before the first frame update
    void Start()
    {       
        jusulso = FindObjectOfType<UI_Jusulso>();
        for (int i = 0; i < progressSlot.Length; i++)
        {
            progressSlot[i] = jusulso.progressList[i];
        }

        slotButton = GetComponentInChildren<Button>();
        slotButton.onClick.AddListener(ClickSlot);
        if(item != null)
        {
            slotImage.sprite = itemImage;
            hasBox= true;
        }
        SetImageAlpha(slotImage);
    }

    public void ClickSlot()
    {
        if (item == null)
        {
            Debug.Log("슬롯에 아이템 프리팹이 없습니다");
        }
        else
        {
            MoveItemToProgressBox();
        }
    }

    private void MoveItemToProgressBox()
    {
        for (int i = 0; i < progressSlot.Length; i++)
        {
            if (!progressSlot[i].hasBox)
            {
                progressSlot[i].ReceiveItem(item);
                slotImage.sprite = null;
                hasBox= false;
                SetImageAlpha(slotImage);
                break;
            }
            else
            {
                Debug.Log("Progress Slot " + i + " 는 이미 차있습니다");
            }
        }
    }
    private void SetImageAlpha(Image image)
    {
        if (!hasBox)
        {
            image.color = new Color(0, 0, 0, 0);
        }
        else
        {
            image.color = new Color(1, 1, 1, 1);
        }
    }
}