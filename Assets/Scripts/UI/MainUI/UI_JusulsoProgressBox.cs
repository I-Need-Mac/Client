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
    private UI_Jusulso jusulso;
    private TimeSpan openTime;
    private TimeSpan duringTime;
    // Start is called before the first frame update
    void Start()
    {
        jusulso = FindObjectOfType<UI_Jusulso>();
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
        TimeSet();
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
                RewardBoxData data = new RewardBoxData();
                TimeSpan timeDifference = jusulso.currentTime - (DateTime)box.open_start_time;
                duringTime = openTime - timeDifference;
                duringTime.Subtract(TimeSpan.FromSeconds(1.0f));
                time.text = string.Format("남은시간 {0:00}:{1:00}:{2:00}", duringTime.Hours, duringTime.Minutes, duringTime.Seconds);
                DebugManager.Instance.PrintDebug(duringTime);
                if (duringTime.TotalSeconds < 0)
                {
                    time.text = "상자 열기";
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    //public IEnumerator UpdateTimer()
    //{
    //    float elapsedTime = 0f;

    //    while (true)
    //    {
    //        if (box != null && box.open_start_time != null)
    //        {
    //            elapsedTime += Time.deltaTime;

    //            if (elapsedTime >= 1.0f)
    //            {
    //                elapsedTime = 0f;

    //                TimeSpan timeDifference = jusulso.currentTime - (DateTime)box.open_start_time;
    //                duringTime = openTime - timeDifference;
    //                TimeSpan oneSecond = TimeSpan.FromSeconds(1);
    //                duringTime -= oneSecond;
    //                //duringTime = duringTime.Subtract(oneSecond);
    //                DebugManager.Instance.PrintDebug(duringTime);

    //                if (duringTime.TotalSeconds < 0)
    //                {
    //                    time.text = "상자 열기";
    //                }
    //                else
    //                {
    //                    time.text = string.Format("남은시간 {0:00}:{1:00}:{2:00}", duringTime.Hours, duringTime.Minutes, duringTime.Seconds);
    //                }
    //            }
    //        }
    //        yield return null;
    //    }
    //}
}
