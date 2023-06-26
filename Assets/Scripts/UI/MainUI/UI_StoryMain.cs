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

        SetData();
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

    void SetData()
    {
        Dictionary<string, Dictionary<string, object>> chapterDataList = UIData.ChapterData;
        if(chapterDataList.Count == 0 || chapterDataList == null)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        // 챕터 리스트 생
        foreach (KeyValuePair<string, Dictionary<string, object>> chapterData in chapterDataList)
        {
            string chapterEleText = LocalizeManager.Instance.GetText("UI_ChapterSelect");

            // chapter list create test
            GameObject chapterGo = Util.FindChild(chapterList.gameObject, "Content", true);
            UI_ChapterElement chapterEle = Util.UILoad<UI_ChapterElement>($"{Define.UiPrefabsPath}/UI_ChapterElement");
            chapterEle.text.text = String.Format(chapterEleText, chapterData.Key);
            GameObject chapterInstance = Instantiate(chapterEle.gameObject) as GameObject;

            chapterInstance.transform.SetParent(chapterGo.transform);
            RectTransform chapterRect = chapterInstance.GetComponent<RectTransform>();
            chapterRect.localScale = new Vector3(1, 1, 1);
            chapterRect.anchoredPosition3D = new Vector3(chapterRect.anchoredPosition3D.x, chapterRect.anchoredPosition3D.y, 0);
        }

        // 서버에서 받은 스테이지가 속한 챕터 선택하기 
        // int currentChapterID = GetChapterID(UIManager.Instance.selectStageID);
        
        SelectChapter(101);

    }

    void SelectChapter(int chapterID)
    {
        Dictionary<string, Dictionary<string, object>> chapterDataList = UIData.ChapterData;
        if (chapterDataList.Count == 0 || chapterDataList == null)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        Dictionary<string, object> chapterInfo = chapterDataList[chapterID.ToString()];
        if( chapterInfo == null || chapterInfo.Count == 0 )
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        // 챕터 이름 챕터 이미지 셋팅 
        foreach (KeyValuePair<string, object> chapterVal in chapterInfo)
        {
            if (chapterVal.Key == UIData.ChapterTableCol.ChapterName.ToString())
            {
                chapterNameSub.text = "<" + chapterVal.Value.ToString() + ">";
                chapterName.text = chapterVal.Value.ToString();
            }
            else if (chapterVal.Key == UIData.ChapterTableCol.ChapterImage.ToString())
            {
                chapterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{chapterVal.Value.ToString()}");
            }
        }

        // 스테이지 리스트 셋팅
        SetStageList(chapterID);
    }

    void SetStageList(int chapterID)
    {
        Dictionary<string, Dictionary<string, object>> stageDataList = UIData.StageData;
        if (stageDataList.Count == 0 || stageDataList == null)
        {
            Debug.LogError("스테이지 테이블 데이터가 없습니다");
        }

        foreach (KeyValuePair<string, Dictionary<string, object>> stageData in stageDataList)
        {
            foreach (KeyValuePair<string, object> stageVal in stageData.Value)
            {
                if (stageVal.Key == UIData.StageTableCol.ChapterCategory.ToString())
                {
                    if(stageVal.Value.ToString() == chapterID.ToString())
                    {
                        string stageEleText = LocalizeManager.Instance.GetText("UI_StageList");

                        // stage list create
                        GameObject stageGo = Util.FindChild(stageList.gameObject, "Content", true);
                        UI_StageElement stageEle = Util.UILoad<UI_StageElement>($"{Define.UiPrefabsPath}/UI_StageElement");
                        stageEle.text.text = String.Format(stageEleText, stageData.Key);
                        GameObject stageInstance = Instantiate(stageEle.gameObject) as GameObject;

                        stageInstance.transform.SetParent(stageGo.transform);
                        RectTransform stageRect = stageInstance.GetComponent<RectTransform>();
                        stageRect.localScale = new Vector3(1, 1, 1);
                        stageRect.anchoredPosition3D = new Vector3(stageRect.anchoredPosition3D.x, stageRect.anchoredPosition3D.y, 0);
                    }
                }
            }
        }
    }

    int GetChapterID(int stageId)
    {
        Dictionary<string, Dictionary<string, object>> stageDataList = UIData.StageData;
        if (stageDataList.Count == 0 || stageDataList == null)
        {
            Debug.LogError("스테이지 테이블 데이터가 없습니다");
        }

        int findChapterID = 0;
        foreach (KeyValuePair<string, Dictionary<string, object>> stageData in stageDataList)
        {
            if( stageData.Key == "" || stageData.Key == null )
            {
                continue;
            }
            
            if(int.Parse(stageData.Key) == stageId)
            {
                foreach (KeyValuePair<string, object> val in stageData.Value)
                {
                    if (val.Key == UIData.StageTableCol.ChapterCategory.ToString())
                    {
                        findChapterID = int.Parse(val.Value.ToString());
                        break;
                    }
                }
            }
        }

        return findChapterID;
    }
    
}
