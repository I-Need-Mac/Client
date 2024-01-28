using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_JusulsoProgressBox : UI_Base
{
    public bool hasBox=false;
    public Item item;
    public Image slotImage;
    public Sprite itemImage;
    public TextMeshProUGUI time;
    // Start is called before the first frame update
    void Start()
    {
        
        if (item != null)
        {
            slotImage.sprite = itemImage;
            hasBox = true;
            time.gameObject.SetActive(true);
        }
        else
        {
            hasBox= false;
            time.gameObject.SetActive(false);
        }
        SetImageAlpha(slotImage);
    }

    public void ReceiveItem(Item newItem)
    {
        if (!hasBox)
        {
            item = newItem;
            slotImage.sprite = itemImage;
            hasBox = true;
            time.gameObject.SetActive(true);
            SetImageAlpha(slotImage);
        }
        else
        {
            Debug.Log("박스가 이미 차 있습니다.");
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
