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

    // StoryTable�� ����ϴ�
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

        // ��ư �̺�Ʈ ���
        for(int i = 0; i < buttonValue.Length; i++)
        {
            BindUIEvent(GetButton(i).gameObject, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
        }

        currentPage = 0;
    }

    public void SetData(StoryBookID id)
    {
        // ������ ����
        bookId = id;
        Dictionary<int, List<object>> pageInfo = UIData.PageTableData[(int)id];
        totalPage = pageInfo.Count;
        
        int pagePos = PAGE_RECT_POSITION * -1;
        int index = 0;
        foreach (KeyValuePair<int, List<object>> pair in pageInfo)
        {
            // ������ ������ �ε�
            UI_Page page = Util.UILoad<UI_Page>($"{Define.UiPrefabsPath}/UI_Page");

            // ���� �ø��ϴ�.
            UI_Page instPage = GameObject.Instantiate<UI_Page>(page);
            // ������ ������ ����
            instPage.SetData(pair.Key, pair.Value);

            GameObject go = instPage.gameObject;

            // �̸� ����
            string reName = go.name.Replace("(Clone)", "").Trim();
            go.name = reName + "_" + index++;

            // å ������ ��ġ
            go.transform.SetParent(this.transform);

            // �ʱ�ȭ�� Ȱ���� ���ֵ��� �մϴ�.
            go.SetActive(false);

            // ������ position ����
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(pagePos, 0);
            pagePos *= -1;

            // ����Ʈ�� �߰�
            pageList.Add(instPage);
        }
        
        // 1�������� ����
        ActivePage(0);
    }

    // �������� Ȱ��ȭ �մϴ�.
    void ActivePage(int page)
    {
        if (page >= totalPage || page < 0)
            return;

        // ��ü ��Ȱ��
        for( int i = 0; i < pageList.Count; i++ )
        {
            pageList[i].gameObject.SetActive(false);
        }

        // �ش� �������� Ȱ��
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

        // ������ ǥ��
        pageText.text = (currentPage + 1) + "/" + (currentPage + PAGE_UNIT);
    }

    // ��ư Ŭ�� �̺�Ʈ
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
                ActivePage(currentPage + PAGE_UNIT);
                break;
            case Buttons.ContentSkip:
                {
                    // �� ���� ��ŵ
                    bool isLast = false;
                    if (pageList[skipPage].IsPageSkippable())
                    {
                        pageList[skipPage].SkipPage(out isLast, false);

                        if (isLast) // ������ ������ ��� ���� ��������
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
                    // ���� ��ŵ
                    bool isLast = false;
                    pageList[currentPage].SkipPage(out isLast, true);
                    pageList[currentPage + 1].SkipPage(out isLast, true);
                }
                
                break;
            case Buttons.Close:
                CloseUI<UI_StoryBook>();
                break;
            default:
                break;
        }
    }
}
