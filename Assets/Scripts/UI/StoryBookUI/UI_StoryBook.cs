using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoryBook : UI_Popup
{
    public enum StoryTableInfo
    {
        StoryTitle,
        StoryPage,
        StoryPath,
        StoryBGM,
    }

    // StoryTable과 맞춥니다
    public enum StoryBookID
    {
        STORY_BOOK_TITLE1 = 1001,
        STORY_BOOK_TITLE2,
        STORY_BOOK_TITLE3,
        STORY_BOOK_TITLE4,
        STORY_BOOK_TITLE5,
    }

    enum Buttons
    {
        PreArrow,
        NextArrow,

        PageSkip,
        AllSkip,

        Close
    }

    enum Images
    {
    }

    enum Texts
    {
    }

    List<UI_Page> pages = new List<UI_Page>();

    void Start()
    {
        Bind<Button>(typeof(Buttons));
        Array buttonValue = Enum.GetValues(typeof(Buttons));

        // 버튼 이벤트 등록
        for(int i = 0; i < buttonValue.Length; i++)
        {
            BindUIEvent(GetButton(i).gameObject, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
        }
    }

    public void SetData(StoryBookID id)
    {
        // 페이지 생성
        Dictionary<int, List<object>> pageInfo = UIData.PageTableData[(int)id];

        int pagePos = -420;
        int index = 0;
        foreach (KeyValuePair<int, List<object>> pair in pageInfo)
        {
            UI_Page page = Util.Load<UI_Page>($"{Define.UiPrefabsPath}/UI_Page");
            page.SetData(pair.Key, pair.Value);
            pages.Add(page);

            // 페이지 position 셋팅
            GameObject go = Util.CreateObject(page.gameObject);
            go.transform.SetParent(this.transform);
            go.name = go.name + "_" + index;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(pagePos, 0);
            pagePos *= -1;

            index++;
        }
    }

    // 버튼 클릭 이벤트
    public void OnClickButton(PointerEventData data)
    {
        Buttons buttonValue = (Buttons)FindEnumValue<Buttons>(data.pointerClick.name);
        if ((int)buttonValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (buttonValue)
        {
            case Buttons.PreArrow:
                break;
            case Buttons.NextArrow:
                break;
            case Buttons.PageSkip:
                break;
            case Buttons.AllSkip:
                break;
            case Buttons.Close:
                CloseUI<UI_StoryBook>();
                break;
            default:
                break;
        }
    }
}
