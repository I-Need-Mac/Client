using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// UI를 관리 합니다. (Create/Open/Close)
public class UIManager : MonoSingleton<UIManager>
{
    public enum UI_Prefab
    {
        UI_StartMain,
        UI_GameMain,

        UI_Login,
        UI_Agreement,

        UI_StoryBook,
    }

    // 메인 UI 우선순위
    int mainUiOrder = 0;
    // 팝업UI 우선순위
    int currentPopupCount = 0;

    // 메인 UI
    UI_StartMain mainUI;
    // UI전체 팝업 목록
    List<UI_Popup> popupList = new List<UI_Popup>();

    // 실시간 팝업 목록
    LinkedList<UI_Popup> currentPopup = new LinkedList<UI_Popup>();

    public void Init()
    {
        // UI관련 테이블을 읽습니다.
        UIData.ReadData();

        // 이벤트 시스템을 추가합니다.
        GameObject go = GameObject.Find("EventSystem");
        if( go == null )
        {   // 이벤트 시스템이 없다면 하나 생성합니다.
            go = new GameObject("EventSystem");
            Util.GetOrAddComponent<EventSystem>(go);
            Util.GetOrAddComponent<StandaloneInputModule>(go);
        }

        // 스택 초기화
        popupList.Clear();

        // ui를 생성합니다.
        Create();

        // 메인 UI는 항상 최상위에 그려집니다.
        mainUiOrder = 0;
        // 초기 우선순위 셋팅
        currentPopupCount = 0;
    }

    // UI를 생성합니다.
    public void Create()
    {
        string[] names = Enum.GetNames(typeof(UI_Prefab));

        // 메인 UI를 생성합니다.
        mainUI = Util.UILoad<UI_StartMain>($"{Define.UiPrefabsPath}/{names[(int)UI_Prefab.UI_StartMain]}");
        if (mainUI == null)
        {
            Debug.Log("mainUI is NULL");
            return;
        }

        // 캔버스를 셋팅합니다.
        SetCanvas(mainUI.gameObject);
        Util.CreateObject(mainUI.gameObject);

        // 전체 UI리스트를 셋팅합니다.
        for ( int i = 0; i < names.Length; i++ )
        {
            if (i == (int)UI_Prefab.UI_StartMain)
                continue;

            // 팝업 UI를 생성합니다.
            UI_Popup popup = Util.UILoad<UI_Popup>($"{Define.UiPrefabsPath}/{names[i]}");
            if ( popup == null )
            {
                Debug.Log(names[i] + " is NULL");
                continue;
            }

            // 캔버스를 셋팅합니다.
            SetCanvas(popup.gameObject);

            // 활성시키지 않은 상태로 초기화 합니다.
            popup.gameObject.SetActive(false);

            // 리스트에 추가합니다.
            popupList.Add(popup);
        }
    }

    // 캔버스를 셋팅합니다.
    private void SetCanvas(GameObject go)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // 우선순위를 0으로 초기화 합니다.
        canvas.sortingOrder = 0;

        CanvasScaler canvasScaler = Util.GetOrAddComponent<CanvasScaler>(go);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(Define.uiScreenWidth, Define.uiScreenHeight);

        Util.GetOrAddComponent<GraphicRaycaster>(go);
    }

    // ui를 엽니다
    public T OpenUI<T>() where T : UI_Base
    {
        UI_Popup popup = FindPopupUI(typeof(T).Name);

        // 이미 띄위져있는 팝업 입니다.
        if (IsCurrentPopup(popup))
            return null;
        
        // 오브젝트로 생성합니다.
        GameObject go = Util.CreateObject(popup.gameObject);

        // 우선순위 지정
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = mainUiOrder + 1;
        
        // 현재 보여지는 팝업 수 증가
        currentPopupCount++;

        go.SetActive(true);

        // 실시간 ui리스트에 추가합니다.
        currentPopup.AddFirst(popup);

        // 팝업전용폴더로 옮겨줍니다.
        GameObject popupRoot = Util.GetOrCreateObjectInActiveScene(Define.UiPopupRoot);
        if (popupRoot == null)
        {
            return null;
        }
        go.transform.SetParent(popupRoot.transform);

        return go.GetComponent<T>();
    }

    // ui를 닫습니다
    public void CloseUI<T>() where T : UI_Base
    {
        // 닫을 팝업이 없습니다.
        if (currentPopup.Count <= 0)
            return;

        // 리스트에서 제외
        LinkedList<UI_Popup>.Enumerator enummerator = currentPopup.GetEnumerator();
        UI_Popup pop = enummerator.Current;
        while(pop == null)
        {
            enummerator.MoveNext();
            pop = enummerator.Current;

            if (pop.name != typeof(T).Name)
                continue;

            currentPopup.Remove(pop);
        }

        // 오브젝트 삭제
        GameObject popupRoot = Util.GetOrCreateObjectInActiveScene(Define.UiPopupRoot);
        GameObject findObject = Util.FindChild(popupRoot, typeof(T).Name);
        Destroy(findObject);

        currentPopupCount--;
    }

    private UI_Popup FindPopupUI(string name)
    {
        // 팝업 리스트에서 해당하는 팝업을 찾습니다.
        for( int i = 0; i < popupList.Count; i++ )
        {
            if(popupList[i].name == name)
            {
                return popupList[i];
            }
        }

        return null;
    }

    private bool IsCurrentPopup(UI_Popup name)
    {
        return currentPopup.Contains(name);
    }
}
