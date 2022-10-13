using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UI_Book : UIPage
{
    enum Buttons
    {
        // 책 페이지 넘김 버튼
        PreArrow,
        NextArrow,
    }

    enum Images
    {
        // 책 이미지
        L_Image1,
        L_Image2,
        L_Image3,

        R_Image1,
        R_Image2,
        R_Image3,
    }

    enum Texts
    {
        // 책 텍스트
        L_Text1,
        L_Text2,
        L_Text3,

        R_Text1,
        R_Text2,
        R_Text3,
    }

    public int page = 0;

    // Start is called before the first frame update
    void Start()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject prev = GetButton((int)Buttons.PreArrow).gameObject;
        BindUIEvent(prev, (PointerEventData data) => { OnPreButtonClicked(data); }, Define.UIEvent.Click);
        GameObject next = GetButton((int)Buttons.NextArrow).gameObject;
        BindUIEvent(next, (PointerEventData data) => { OnNextButtonClicked(data); }, Define.UIEvent.Click);

        SetPageText();
    }

    public void SetPageText()
    {
        List<object> pageList = new List<object>();
        UIManager.Instance.page.TryGetValue(page, out pageList);
        Get<Text>((int)Texts.L_Text1).text = pageList[1].ToString();
        Get<Text>((int)Texts.L_Text2).text = pageList[2].ToString();
        Get<Text>((int)Texts.L_Text3).text = pageList[3].ToString();

        Get<Text>((int)Texts.R_Text1).text = pageList[1].ToString();
        Get<Text>((int)Texts.R_Text2).text = pageList[2].ToString();
        Get<Text>((int)Texts.R_Text3).text = pageList[3].ToString();
    }

    public void OnPreButtonClicked(PointerEventData data)
    {
        int active = 0;
        for (int i = 0; i < UIManager.Instance.bookPageObj.Length; i++)
        {
            if(UIManager.Instance.bookPageObj[i].active)
            {
                active = i;
                break;
            }
        }
        
        for (int i = 0; i < UIManager.Instance.bookPageObj.Length; i++)
        {
            if (!UIManager.Instance.bookPageObj[i])
                continue;

            UIManager.Instance.bookPageObj[i].SetActive(false);
        }

        if (active <= 0)
            active = 0;
        else
            active -= 1;

        UIManager.Instance.bookPageObj[active].SetActive(true);

        Debug.Log(active + "Page");
    }

    public void OnNextButtonClicked(PointerEventData data)
    {
        int active = 0;
        for (int i = 0; i < UIManager.Instance.bookPageObj.Length; i++)
        {
            if (UIManager.Instance.bookPageObj[i].active)
            {
                active = i;
                break;
            }
        }

        for (int i = 0; i < UIManager.Instance.bookPageObj.Length; i++)
        {
            if (!UIManager.Instance.bookPageObj[i])
                continue;
            
            UIManager.Instance.bookPageObj[i].SetActive(false);
        }

        if (active >= UIManager.Instance.page.Count-1)
            active = UIManager.Instance.page.Count - 1;
        else
            active += 1;

        UIManager.Instance.bookPageObj[active].SetActive(true);

        Debug.Log(active + "Page");
    }
}