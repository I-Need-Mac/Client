using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoryMain : UI_Popup
{
    enum Images
    {
        BackBtn,
        StartButton,
    }

    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI chapterStageTitle;
    [SerializeField]
    TextMeshProUGUI chapterNameSub;

    [SerializeField]
    TextMeshProUGUI chapterName;

    [SerializeField]
    GameObject chapterList;
    [SerializeField]
    GameObject stageList;
    
    [SerializeField]
    GameObject chapterImage;


    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        title.text = LocalizeManager.Instance.GetText("UI_StoryMode");

        // string stageTitleText = LocalizeManager.Instance.GetText("UI_CurrentStage");
        // stageTitle.text = String.Format(stageTitleText, 1, 1);

        string eleText = LocalizeManager.Instance.GetText("UI_StageList");

        SetData();

        GameObject go = Util.FindChild(stageList.gameObject, "Content", true);
        UI_StageElement ele = Util.UILoad<UI_StageElement>($"{Define.UiPrefabsPath}/UI_StageElement");
        ele.text.text = String.Format(eleText, "첫");
        GameObject instance = Instantiate(ele.gameObject) as GameObject;

        instance.transform.SetParent(go.transform);
        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);

        UI_StageElement ele2 = Util.UILoad<UI_StageElement>($"{Define.UiPrefabsPath}/UI_StageElement");
        ele2.text.text = String.Format(eleText, 2);
        GameObject instance2 = Instantiate(ele2.gameObject) as GameObject;

        instance2.transform.SetParent(go.transform);
        RectTransform rect2 = instance2.GetComponent<RectTransform>();
        rect2.localScale = new Vector3(1, 1, 1);

        UI_StageElement ele3 = Util.UILoad<UI_StageElement>($"{Define.UiPrefabsPath}/UI_StageElement");
        ele3.text.text = String.Format(eleText, 3);
        GameObject instance3 = Instantiate(ele3.gameObject) as GameObject;

        instance3.transform.SetParent(go.transform);
        RectTransform rect3 = instance3.GetComponent<RectTransform>();
        rect3.localScale = new Vector3(1, 1, 1);
    }

    void SetData()
    {
        Dictionary<string, Dictionary<string, object>> chapterList = UIData.ChapterData;
        if(chapterList.Count == 0 || chapterList == null)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        Dictionary<string, Dictionary<string, object>> stageList = UIData.StageData;
        if (stageList.Count == 0 || stageList == null)
        {
            Debug.LogError("스테이지 테이블 데이터가 없습니다");
        }

        // 스토리 모드 리스트 생성
        foreach (KeyValuePair<string, Dictionary<string, object>> chapterData in chapterList)
        {
            int chapterID = int.Parse(chapterData.Key);
            
            foreach(KeyValuePair<string, object> chapterVal in chapterData.Value)
            {
                if(chapterVal.Key == UIData.ChapterTableCol.ChapterName.ToString())
                {
                    chapterNameSub.text = chapterVal.Value.ToString();
                    chapterName.text = chapterVal.Value.ToString();
                }
                else if(chapterVal.Key == UIData.ChapterTableCol.ChapterImage.ToString())
                {
                    chapterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{chapterVal.Value.ToString()}");
                }
            }

            foreach(KeyValuePair<string, Dictionary<string, object>> stageData in stageList)
            {

            }
        }

        //foreach (KeyValuePair<string, Dictionary<string, object>> data in stageData)
        //{
        //    foreach (KeyValuePair<string, object> val in data.Value)
        //    {
        //        if (val.Key == UIData.StageTableCol.StageName.ToString())
        //        {
        //            stageSub.text = val.Value.ToString();
        //        }
        //    }
        //}
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
                this.CloseUI<UI_StoryMain>();
                break;
            case Images.StartButton:
                UI_StoryBook storyBook = UIManager.Instance.OpenUI<UI_StoryBook>();
                storyBook.SetData(UI_StoryBook.StoryBookID.STORY_BOOK_TITLE1);
                break;
            default:
                break;
        }
    }
}
