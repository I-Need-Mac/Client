using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 개선할것
 * 1. UI를 올릴떄마다 리소스를 로드하는것이 부담되는지 생각해보기
 * 만약 부담이 된다면 게임 시작시 미리 리소스를 로드해두고 셋팅할 때 로드해둔 리소스를 쓰도록한다.
 * 2. UI마다 캔버스가 붙어있는것 개선
 * 지금 코드로는 UI마다 캔버스가 다 붙어있어야 하므로 하나의 캔버스에 하위로 붙여서 할 수 있도록 개선필요
 * 3. 팝업 코드 기능 변경 필요 (PC에 맞게 수정)
 * 
 * 추가할것
 * 팝업 기능 분류해야함 (전체 팝업 / 인벤 팝업 / Message팝업 등
 */

// UI를 관리 합니다.
public class UIManager : MonoSingleton<UIManager>
{
    // UI프리팹 저장 경로
    const string UiPrefabsPath = "Prefabs/UI";
    const string UiPageRoot = "@UI_Page_Root";
    const string UiPopupRoot = "@UI_Popup_Root";

    // 페이지 최대치는 5개로 합니다.
    const int MaxPageOrder = 50;

    // ui render 우선순위
    int pageOrder = 0;
    int popupOrder = 0;

    // UI페이지 관리
    public UIPage[] pageAry = new UIPage[MaxPageOrder];
    public UI_Book[] bookPageAry = new UI_Book[MaxPageOrder];
    public GameObject[] bookPageObj = new GameObject[MaxPageOrder];

    // UI팝업 관리
    Stack<UIPopup> popupStack = new Stack<UIPopup>();

    public Dictionary<int, List<object>> book = new Dictionary<int, List<object>>();
    public Dictionary<int, List<object>> page = new Dictionary<int, List<object>>();

    public void Init()
    {
        popupStack.Clear();

        popupOrder = MaxPageOrder;
    }

    #region POPUP
    public void SetPopupUI<T>(string name = null) where T : UIPopup
    {
        if (name == null)
            name = typeof(T).ToString();

        T prefab = Resources.Load<T>($"{UiPrefabsPath}/{name}");
        if (prefab == null)
            return;

        // 인스턴스를 생성, 씬에 올립니다.
        GameObject go = GameObject.Instantiate(prefab.gameObject);
        if (go == null)
            return;

        // T컴포넌트를 가져옵니다.
        T ui = Util.GetOrAddComponent<T>(go);
        if (ui == null)
            return;

        // 스택에 추가합니다.
        popupStack.Push(ui);

        // 우선순위를 정렬합니다.
        // 캔버스에 접근합니다.
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = popupOrder;
        popupOrder++;

        // 팝업전용폴더로 옮겨줍니다.
        GameObject popupRoot = Util.CreateObject(UiPopupRoot);
        if (popupRoot == null)
        {
            return;
        }

        go.transform.SetParent(popupRoot.transform);
    }

    public void ClosePopupUI<T>() where T : UIPopup
    {
        if (popupStack.Count == 0)
            return;

        Type type = typeof(T);

        if (popupStack.Peek().GetType() != type)
        {
            Debug.Log($"Fail Close {type.ToString()} UI");
            return;
        }

        CloseTopPopupUI();
    }

    public void CloseTopPopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UIPopup ui = popupStack.Pop();
        if (ui == null)
            return;

        GameObject.Destroy(ui.gameObject);
        popupOrder--;

        if (popupOrder == 1)
        {   // 마지막 팝업은 삭제할 때 팝업전용 오브젝트도 같이 삭제합니다.
            GameObject popupRoot = Util.CreateObject(UiPopupRoot);
            if (popupRoot == null)
            {
                return;
            }

            GameObject.Destroy(popupRoot);
        }
    }
    #endregion

    #region PAGE
    // 페이지를 셋팅합니다.
    public void SetPageUI<T>(string name = null) where T : UIPage
    {
        if (name == null)
            name = typeof(T).ToString();

        GameObject prefab = Resources.Load<GameObject>($"{UiPrefabsPath}/{name}");
        if (prefab == null)
            return;

        //T prefab = Resources.Load<T>($"{UiPrefabsPath}/{name}");
        //if (prefab == null)
        //    return;

        // 인스턴스를 생성, 씬에 올립니다.
        GameObject go = GameObject.Instantiate(prefab);
        if (go == null)
            return;

        // T컴포넌트를 가져옵니다.
        T ui = Util.GetOrAddComponent<T>(go);
        if (ui == null)
            return;

        // 페이지에 등록합니다.
        pageAry[pageOrder] = ui;
        
        // 우선순위를 정렬합니다.
        // 캔버스에 접근합니다.
        //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //canvas.sortingOrder = pageOrder;
        //pageOrder++;

        // 팝업전용폴더로 옮겨줍니다.
        GameObject popupRoot = Util.CreateObject(UiPageRoot);
        if (popupRoot == null)
        {
            return;
        }

        GameObject obj = GameObject.Find("Canvas");
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.SetParent(popupRoot.transform);

        Canvas can = obj.GetComponent<Canvas>();
        can.sortingOrder = pageOrder;
        pageOrder++;

        go.transform.SetParent(obj.transform);
    }

    // 페이지를 셋팅합니다.
    public void SetBookPage<T>(string name = null) where T : UI_Book
    {
        if (name == null)
            name = typeof(T).ToString();

        GameObject prefab = Resources.Load<GameObject>($"{UiPrefabsPath}/{name}");
        if (prefab == null)
            return;

        //T prefab = Resources.Load<T>($"{UiPrefabsPath}/{name}");
        //if (prefab == null)
        //    return;

        // 인스턴스를 생성, 씬에 올립니다.
        GameObject go = GameObject.Instantiate(prefab);
        if (go == null)
            return;

        bookPageObj[pageOrder] = go;
        go.SetActive(false);

        // T컴포넌트를 가져옵니다.
        T ui = Util.GetOrAddComponent<T>(go);
        if (ui == null)
            return;

        // 페이지에 등록합니다.
        bookPageAry[pageOrder] = ui;

        // 우선순위를 정렬합니다.
        // 캔버스에 접근합니다.
        //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //canvas.sortingOrder = pageOrder;
        //pageOrder++;

        // 팝업전용폴더로 옮겨줍니다.
        GameObject popupRoot = Util.CreateObject(UiPageRoot);
        if (popupRoot == null)
        {
            return;
        }

        GameObject obj = GameObject.Find("Canvas");
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.SetParent(popupRoot.transform);

        Canvas can = obj.GetComponent<Canvas>();
        can.sortingOrder = pageOrder;
        pageOrder++;

        go.transform.SetParent(obj.transform);
    }

    // 메인 씬 UI를 
    public void DestroyPageUI<T>() where T : UIPage
    {
        // 파괴할 일이있나?
        GameObject.Destroy(pageAry[pageOrder].gameObject);
        pageAry[pageOrder] = null;
        pageOrder--;

        GameObject popupRoot = Util.CreateObject(UiPopupRoot);
        if (popupRoot == null)
        {
            return;
        }

        GameObject.Destroy(popupRoot);
    }
    #endregion
}
