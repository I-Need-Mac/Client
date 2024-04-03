using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UIStatus;

public class UI_Jusulso : UI_Popup
{
    enum GameObjects
    {
        Close,
        Left,
        Right
    }
    [SerializeField]
    TextMeshProUGUI jusulso_Title;
    [SerializeField]
    TextMeshProUGUI progress_Box;
    [SerializeField]
    TextMeshProUGUI possesion_Box;
    [SerializeField]
    TextMeshProUGUI close_Text;

    public UnityEngine.UI.Image right;
    public UnityEngine.UI.Image left;
    public Sprite[] lockSprite;

    [SerializeField]
    GameObject slot_Page;
    [SerializeField]
    Transform possesionSlot;
    [SerializeField]
    List<UI_JusulsoPossesionSlot> slotList = new List<UI_JusulsoPossesionSlot>();
    [SerializeField]
    GameObject progressSlot;
    [SerializeField]
    public List<UI_JusulsoProgressBox> progressList = new List<UI_JusulsoProgressBox>(); 
    private List<List<UI_JusulsoPossesionSlot>> pageList = new List<List<UI_JusulsoPossesionSlot>>();
    public List<GameObject> pageGamObject = new List<GameObject>();

    public List<UI_Jusulso_Box> boxList = new List<UI_Jusulso_Box>();

    public int currentPageIndex = 0;

    public DateTime currentTime;

    void Start()
    {
        RequestBox();
        jusulso_Title.text = LocalizeManager.Instance.GetText("UI_Sorcere_Title");
        progress_Box.text = LocalizeManager.Instance.GetText("UI_Sorcere_MyBoxes");
        possesion_Box.text = LocalizeManager.Instance.GetText("UI_Sorcere_AllBox");
        close_Text.text = LocalizeManager.Instance.GetText("UI_Result_Exit");
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
        UI_JusulsoProgressBox progressBox = Util.UILoad<UI_JusulsoProgressBox>(Define.UiPrefabsPath + "/UI_JusulsoProgressBox");
        for(int i = 0; i<3; i++)
        {
            UI_JusulsoProgressBox slot = Instantiate(progressBox);
            slot.GetComponent<UI_JusulsoProgressBox>();
            slot.transform.SetParent(progressSlot.transform);
            progressList.Add(slot);
        }

        SetProgressSlotPos();

    }

    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case GameObjects.Close:
                UIManager.Instance.CloseUI<UI_Jusulso>();
                break;
            case GameObjects.Left:
                PreviousPage();
                break;
            case GameObjects.Right:
                NextPage();
                break;
            default:
                break;
        }
    }
    private void SetSlotPos()
    {
        int rowSize = 8;
        float horizontalSpacing = 154;
        float verticalSpacing = 140;
        int maxSlotCount = 16; // 최대 슬롯 개수

        float initialXPos = -540;
        float initialYPos = -150;

        int row = 0; // 초기 행

        for (int i = 0; i < slotList.Count; i++)
        {
            RectTransform r = slotList[i].GetComponent<RectTransform>();
            int col = i % rowSize;

            float xPos = initialXPos + col * horizontalSpacing;
            float yPos = initialYPos - row * verticalSpacing;

            // 16의 배수일 때 초기 위치로 설정
            if (i > 0 && i % maxSlotCount == 0)
            {
                yPos = initialYPos; // 초기 yPos로 설정
                row = 0; // 첫 번째 행으로 리셋
            }

            r.anchoredPosition3D = new Vector3(xPos, yPos, 0);

            // 16의 배수가 아니면서 한 행이 다 찼을 때 다음 행으로 이동
            if (i > 0 && (i + 1) % rowSize == 0)
            {
                row++;
            }
        }
    }
    private void SetProgressSlotPos()
    {
        float spacing = 392;
        for (int i = 0; i < progressList.Count; i++)
        {
            RectTransform r = progressList[i].GetComponent<RectTransform>();
            float pos = -392 + i * spacing;
            r.anchoredPosition3D = new Vector3(pos, 178, 0);
        }
    }

    private void NewSlotPage()
    {
        GameObject slotPage = Instantiate(slot_Page,possesionSlot);
        for (int i = 0; i < 16; i++)
        {
            UI_JusulsoPossesionSlot possesionSlot = Util.UILoad<UI_JusulsoPossesionSlot>(Define.UiPrefabsPath + "/UI_JusulsoPossesionSlot");
            GameObject slotGameObject = Instantiate(possesionSlot.gameObject);
            UI_JusulsoPossesionSlot slotComponent = slotGameObject.GetComponent<UI_JusulsoPossesionSlot>();
            slotGameObject.transform.SetParent(slotPage.transform);
            slotList.Add(slotComponent); // UI_JusulsoPossesionSlot 타입으로 캐스팅하여 추가
        }
        SetSlotPos();
        pageGamObject.Add(slotPage);
        pageList.Add(slotList);

    }
    private void SwitchPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < pageList.Count)
        {
            currentPageIndex = pageIndex;

            for (int i = 0; i < pageList.Count; i++)
            {
                pageGamObject[i].SetActive(i == currentPageIndex);
            }

            ArrowImageControl(pageIndex);
        }
    }
    private void ArrowImageControl(int pageIndex)
    {
        if (pageList.Count >=2&&pageIndex == pageList.Count - 1)
        {
            right.sprite = lockSprite[0];
            right.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
            left.sprite = lockSprite[1];
            left.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
            DebugManager.Instance.PrintDebug("if");
        }
        else if (pageList.Count >=2&&pageIndex == 0)
        {
            right.sprite = lockSprite[1];
            right.rectTransform.localScale = new Vector3(1f, 1f, 1f);
            left.sprite = lockSprite[0];
            left.rectTransform.localScale = new Vector3(1f, 1f, 1f);
            
            DebugManager.Instance.PrintDebug("else if");
        }
        else
        {
            right.sprite = lockSprite[1];
            right.rectTransform.localScale = new Vector3(1f, 1f, 1f);
            left.sprite = lockSprite[1];
            left.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
            DebugManager.Instance.PrintDebug("else");
        }
    }
    private void NextPage()
    {
        int nextPageIndex = currentPageIndex + 1;
        if (nextPageIndex < pageList.Count)
        {
            SwitchPage(nextPageIndex);
        }

    }
    private void PreviousPage()
    {
        int previousPageIndex = currentPageIndex - 1;
        if (previousPageIndex >= 0)
        {
            SwitchPage(previousPageIndex);
        }
    }



    public async void RequestBox()
    {
        OwnBoxResult result = await APIManager.Instance.GetBox();
        Debug.Log(result.data.userRewardBoxes.Count);
        currentTime = result.data.current_time;
        int totalBoxes = result.data.userRewardBoxes.Count;
        int totalPages = (totalBoxes + 15) / 16;
        while (totalPages > pageList.Count)
        {
            NewSlotPage();
        }

        for (int i = 0; i < result.data.userRewardBoxes.Count; i++)
        {
            // UI_Jusulso_Box 프리팹을 Resources에서 로드하여 인스턴스화
            UI_Jusulso_Box newBox = Instantiate(Resources.Load<UI_Jusulso_Box>("Prefabs/InGame/Item/Item_Box"), transform);
            boxList.Add(newBox);
            boxList[i].SetData(result.data.userRewardBoxes[i]);
            int progressIndex = 0;
            if (boxList[i].open_start_time == null)
            {
                slotList[i].SetItem(newBox);
            }
            else if (boxList[i].open_start_time != null)
            {
                while (progressIndex < progressList.Count && progressList[progressIndex].hasBox)
                {
                    progressIndex++;
                }
                if (progressIndex < progressList.Count)
                {
                    progressList[progressIndex].SetItem(newBox);
                    progressIndex++;
                }
            }
        }

        // 페이지가 2개 이상인 경우 두 번째 페이지부터 비활성화
        if (pageGamObject.Count >= 2)
        {
            for (int i = 1; i < pageGamObject.Count; i++)
            {
                pageGamObject[i].SetActive(false);
            }
        }
    }
}
