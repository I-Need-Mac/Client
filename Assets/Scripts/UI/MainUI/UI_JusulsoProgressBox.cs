using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_JusulsoProgressBox : UI_Base
{
    public bool hasBox=false;
    public UI_Jusulso_Box box;
    public Image slotImage;
    public Sprite itemImage;
    public TextMeshProUGUI time;
    private Button slotButton;
    public DateTime currentTime;

    private TimeSpan openTime;
    private TimeSpan duringTime;
    // Start is called before the first frame update
    void Start()
    {
        slotButton = GetComponentInChildren<Button>();
        slotButton.onClick.AddListener(RequestBoxOpen);
        if (box != null)
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
        openTime = new TimeSpan(0,100,0);
        StartCoroutine(UpdateTimer());
        
        SetImageAlpha();
    }

    public void ReceiveItem(UI_Jusulso_Box newItem)
    {
        if (!hasBox)
        {
            box = newItem;
            slotImage.sprite = itemImage;
            hasBox = true;
            time.gameObject.SetActive(true);
            SetImageAlpha();
            StartCoroutine(UpdateTimer());
        }
        else
        {
            Debug.Log("박스가 이미 차 있습니다.");
        }
    }
    private void SetImageAlpha()
    {
        if (box==null)
        {
            slotImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            slotImage.color = new Color(1, 1, 1, 1);
        }
    }
    async void RequestBoxOpen()
    {
        if(box != null)
        {
            BoxOpen boxOpen = await APIManager.Instance.BoxOpen(box.id);
            if (boxOpen.statusCode == 200)
            {
                DebugManager.Instance.PrintDebug("박스 열기");
                StartCoroutine(UpdateTimer());
            }

        }
    }
    public void TimeSet()
    {
        if(box !=null)
        {
            if(box.box_type == 1)
            {
                openTime = new TimeSpan(0, 1, 0);
            }
            else
            {
                openTime = new TimeSpan(0, 100, 0);
            }
        }
    }
    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            if (box != null && box.open_start_time != null)
            {
                currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - (DateTime)box.open_start_time;
                duringTime = openTime - timeDifference;           
                time.text = string.Format("남은시간 {0:00}:{1:00}:{2:00}", duringTime.Hours, duringTime.Minutes, duringTime.Seconds);

                if (duringTime.TotalSeconds < 0)
                {
                    time.text = "상자 열기";
                }
            }
            
            yield return new WaitForSeconds(1.0f);
        }
    }
}
