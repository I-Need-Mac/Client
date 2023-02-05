using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoryBook : UI_Popup
{
    const int PAGE_UNIT = 2;
    const int PAGE_RECT_POSITION = 420;

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

        ContentSkip,
        PageSkip,

        Close
    }

    enum Images
    {
    }

    enum Texts
    {
        PageInfo
    }

    [SerializeField]
    Text pageText;
    [SerializeField]
    Image background;

    List<UI_Page> pageList = new List<UI_Page>();
    int totalPage = 0;
    int currentPage = 0;
    int skipPage = 0;

    StoryBookID bookId = 0;

    void Start()
    {
        Bind<Button>(typeof(Buttons));
        Array buttonValue = Enum.GetValues(typeof(Buttons));

        // 버튼 이벤트 등록
        for(int i = 0; i < buttonValue.Length; i++)
        {
            BindUIEvent(GetButton(i).gameObject, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
        }

        currentPage = 0;
    }

    public void SetData(StoryBookID id)
    {
        // 페이지 생성
        bookId = id;
        Dictionary<int, List<object>> pageInfo = UIData.PageTableData[(int)id];
        totalPage = pageInfo.Count;
        
        int pagePos = PAGE_RECT_POSITION * -1;
        int index = 0;
        foreach (KeyValuePair<int, List<object>> pair in pageInfo)
        {
            // 페이지 데이터 로드
            UI_Page page = Util.UILoad<UI_Page>($"{Define.UiPrefabsPath}/UI_Page");

            // 씬에 올립니다.
            UI_Page instPage = GameObject.Instantiate<UI_Page>(page);
            // 페이지 데이터 셋팅
            instPage.SetData(pair.Key, pair.Value);

            GameObject go = instPage.gameObject;

            // 이름 셋팅
            string reName = go.name.Replace("(Clone)", "").Trim();
            go.name = reName + "_" + index++;

            // 책 하위에 위치
            go.transform.SetParent(this.transform);

            // 초기화시 활성은 꺼주도록 합니다.
            go.SetActive(false);

            // 페이지 position 셋팅
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(pagePos, 0);
            rt.anchoredPosition3D = new Vector3(pagePos, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            pagePos *= -1;

            // 리스트에 추가
            pageList.Add(instPage);
        }
        
        // 1페이지로 셋팅
        ActivePage(0);
    }

    // 페이지를 활성화 합니다.
    void ActivePage(int page)
    {
        if (page >= totalPage)
        {
            currentPage = totalPage;
            return;
        }

        if(page < 0)
        {
            currentPage = 0;
            return;
        }

        // 전체 비활성
        for( int i = 0; i < pageList.Count; i++ )
        {
            pageList[i].gameObject.SetActive(false);
        }

        // 해당 페이지만 활성
        pageList[page].gameObject.SetActive(true);
        pageList[page + 1].gameObject.SetActive(true);

        if(pageList[page].TYPE == UI_Page.PageType.Picture)
        {
            pageList[page + 1].ActivePage();
            skipPage = page + 1;
        }
        else
        {
            pageList[page].ActivePage();
            pageList[page + 1].ClearPage();
            skipPage = page;
        }

        currentPage = page;

        // 페이지 표시
        pageText.text = (currentPage + 1) + "/" + (currentPage + PAGE_UNIT);
    }

    // 버튼 클릭 이벤트
    void OnClickButton(PointerEventData data)
    {
        Buttons buttonValue = (Buttons)FindEnumValue<Buttons>(data.pointerClick.name);
        if ((int)buttonValue < -1)
            return;

        switch (buttonValue)
        {
            case Buttons.PreArrow:
                ActivePage(currentPage - PAGE_UNIT);
                break;
            case Buttons.NextArrow:
                if (currentPage >= totalPage)
                {
                    // 스토리 끝나면 인게임으로 고공
                    // 선택된 스테이지id, 캐릭터id 전투씬으로 넘기깅
                    //UIManager.Instance.selectCharacterID;
                    //UIManager.Instance.selectStageID;
                }

                ActivePage(currentPage + PAGE_UNIT);
                
                break;
            case Buttons.ContentSkip:
                {
                    // 한 문단 스킵
                    bool isLast = false;
                    if (pageList[skipPage].IsPageSkippable())
                    {
                        pageList[skipPage].SkipPage(out isLast, false);

                        if (isLast) // 마지막 문단인 경우 다음 페이지로
                        {
                            skipPage++;

                            if (skipPage == currentPage + 1)
                            {
                                pageList[skipPage].ActivePage();
                            }
                            else if (skipPage == currentPage + PAGE_UNIT)
                            {
                                if (pageList[skipPage - 1].TYPE == UI_Page.PageType.Text)
                                {
                                    skipPage--;
                                    break;
                                }

                                ActivePage(currentPage + PAGE_UNIT);
                            }
                        }
                    }
                    else
                    {
                        if(pageList[skipPage].TYPE == UI_Page.PageType.Picture)
                        {

                        }
                        ActivePage(currentPage + PAGE_UNIT);
                    }
                }

                break;
            case Buttons.PageSkip:
                {
                    // 한쪽 스킵
                    bool isLast = false;
                    pageList[currentPage].SkipPage(out isLast, true);
                    pageList[currentPage + 1].SkipPage(out isLast, true);
                }
                
                break;
            case Buttons.Close:
                UIManager.Instance.CloseUI<UI_StoryBook>();
                break;
            default:
                break;
        }
    }
}
