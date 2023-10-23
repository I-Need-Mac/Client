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
        if (chapterDataList.Count == 0 || chapterDataList == null)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        string chapterEleText = LocalizeManager.Instance.GetText("UI_ChapterSelect");

        // 서버에서 받은 스테이지가 속한 챕터 아이디 get
        string currentChapterID = GetChapterID(UIManager.Instance.selectStageID.ToString());

        // create chapter list
        foreach (KeyValuePair<string, Dictionary<string, object>> chapterData in chapterDataList)
        {
            try
            {
                // get chapter number text
                chapterData.Value.TryGetValue(UIData.ChapterTableCol.ChapterHeader.ToString(), out object chapterHeaderLocalStr);
                string chapterHeader = LocalizeManager.Instance.GetText(chapterHeaderLocalStr.ToString());

                // create chapter element
                UI_ChapterElement chapterEle = Util.UILoad<UI_ChapterElement>($"{Define.UiPrefabsPath}/UI_ChapterElement");
                chapterEle.text.text = String.Format(chapterEleText, chapterHeader);
                GameObject chapterInstance = Instantiate(chapterEle.gameObject) as GameObject;

                // setting chapter element
                GameObject chapterGo = Util.FindChild(chapterList.gameObject, "Content", true);
                chapterInstance.transform.SetParent(chapterGo.transform);
                RectTransform chapterRect = chapterInstance.GetComponent<RectTransform>();
                chapterRect.localScale = new Vector3(1, 1, 1);
                chapterRect.anchoredPosition3D = new Vector3(chapterRect.anchoredPosition3D.x, chapterRect.anchoredPosition3D.y, 0);
            }catch(Exception e) { 
                continue;
            }

        }

        // 챕터 선택
        SelectChapter(currentChapterID);
    }

    void DrawStoryModeList()
    {
        // story mode info
        // 서버에서 받은 스테이지가 속한 챕터 아이디 get
        string currentChapterID = GetChapterID(UIManager.Instance.selectStageID.ToString());

        Dictionary<string, Dictionary<string, object>> chapterDataList = UIData.ChapterData;
        if (chapterDataList.Count == 0 || chapterDataList == null)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        string chapterEleText = LocalizeManager.Instance.GetText("UI_ChapterSelect");

        // draw chapter
        foreach (KeyValuePair<string, Dictionary<string, object>> chapterData in chapterDataList)
        {
            // get chapter number text
            chapterData.Value.TryGetValue(UIData.ChapterTableCol.ChapterHeader.ToString(), out object chapterHeaderLocalStr);
            string chapterHeader = LocalizeManager.Instance.GetText(chapterHeaderLocalStr.ToString());

            // create chapter element
            UI_ChapterElement chapterEle = Util.UILoad<UI_ChapterElement>($"{Define.UiPrefabsPath}/UI_ChapterElement");
            chapterEle.text.text = String.Format(chapterEleText, chapterHeader);
            GameObject chapterInstance = Instantiate(chapterEle.gameObject) as GameObject;

            // setting chapter element
            GameObject chapterGo = Util.FindChild(chapterList.gameObject, "Content", true);
            chapterInstance.transform.SetParent(chapterGo.transform);
            RectTransform chapterRect = chapterInstance.GetComponent<RectTransform>();
            chapterRect.localScale = new Vector3(1, 1, 1);
            chapterRect.anchoredPosition3D = new Vector3(chapterRect.anchoredPosition3D.x, chapterRect.anchoredPosition3D.y, 0);
        }

        // draw stage
    }

    void SelectChapter(string chapterID)
    {
        // 챕터 리스트 가져오기
        Dictionary<string, Dictionary<string, object>> chapterDataList = UIData.ChapterData;
        if (chapterDataList.Count == 0 || chapterDataList == null)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        // 해당 챕터 내용 가져오기
        Dictionary<string, object> chapterInfo = chapterDataList[chapterID.ToString()];
        if (chapterInfo == null || chapterInfo.Count == 0)
        {
            Debug.LogError("챕터 테이블 데이터가 없습니다");
        }

        // set chapter number 
        string headerText = chapterInfo[UIData.ChapterTableCol.ChapterHeader.ToString()].ToString();
        string headerLocalText = LocalizeManager.Instance.GetText(headerText);
        string chapterEleText = LocalizeManager.Instance.GetText("UI_ChapterSelect");

        Dictionary<string, object> selectStageData = UIData.StageData[UIManager.Instance.selectStageID.ToString()];
        string stageHeaderText = selectStageData[UIData.StageTableCol.StageHeader.ToString()].ToString();
        string stageHeaderLocalText = LocalizeManager.Instance.GetText(stageHeaderText);

        string stageEleText = LocalizeManager.Instance.GetText("UI_StageList");
        chapterStageTitle.text = String.Format(chapterEleText, headerLocalText)
            + " " + String.Format(stageEleText, stageHeaderLocalText);

        // set chapter name
        string nameText = chapterInfo[UIData.ChapterTableCol.ChapterName.ToString()].ToString();
        string nameLocalText = LocalizeManager.Instance.GetText(nameText);
        chapterNameSub.text = "<" + nameLocalText + ">";
        chapterName.text = nameLocalText;

        // set chapter image
        string imageText = chapterInfo[UIData.ChapterTableCol.ChapterImage.ToString()].ToString();
        chapterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{imageText}");

        // 해당 챕터를 구성하는 스테이지 셋팅
        SetStageList(chapterID);
    }

    void SetStageList(string chapterID)
    {
        // 스테이지 리스트 가져오기
        Dictionary<string, Dictionary<string, object>> stageDataList = UIData.StageData;
        if (stageDataList.Count == 0 || stageDataList == null)
        {
            Debug.LogError("스테이지 테이블 데이터가 없습니다");
        }

        string stageEleText = LocalizeManager.Instance.GetText("UI_StageList");

        foreach (KeyValuePair<string, Dictionary<string, object>> stageData in stageDataList)
        {
            if (stageData.Value.Count == 0)
            {
                continue;
            }

            if (stageData.Value[UIData.StageTableCol.ChapterCategory.ToString()].ToString() == chapterID)
            {
                // stage list create
                GameObject stageGo = Util.FindChild(stageList.gameObject, "Content", true);
                UI_StageElement stageEle = Util.UILoad<UI_StageElement>($"{Define.UiPrefabsPath}/UI_StageElement");

                string stageHeader = stageData.Value[UIData.StageTableCol.StageHeader.ToString()].ToString();
                string stageHeaderLocalText = LocalizeManager.Instance.GetText(stageHeader);
                stageEle.text.text = String.Format(stageEleText, stageHeaderLocalText);
                GameObject stageInstance = Instantiate(stageEle.gameObject) as GameObject;

                stageInstance.transform.SetParent(stageGo.transform);
                RectTransform stageRect = stageInstance.GetComponent<RectTransform>();
                stageRect.localScale = new Vector3(1, 1, 1);
                stageRect.anchoredPosition3D = new Vector3(stageRect.anchoredPosition3D.x, stageRect.anchoredPosition3D.y, 0);
            }
        }
    }

    string GetChapterID(string stageId)
    {
        Dictionary<string, Dictionary<string, object>> stageDataList = UIData.StageData;
        if (stageDataList.Count == 0 || stageDataList == null)
        {
            Debug.LogError("스테이지 테이블 데이터가 없습니다");
        }

        string findChapterID = "";
        foreach (KeyValuePair<string, Dictionary<string, object>> stageData in stageDataList)
        {
            if (stageData.Key == "" || stageData.Key == null)
            {
                continue;
            }

            if (stageData.Key == stageId)
            {
                foreach (KeyValuePair<string, object> val in stageData.Value)
                {
                    if (val.Key == UIData.StageTableCol.ChapterCategory.ToString())
                    {
                        findChapterID = val.Value.ToString();
                        break;
                    }
                }
            }
        }

        return findChapterID;
    }
}
