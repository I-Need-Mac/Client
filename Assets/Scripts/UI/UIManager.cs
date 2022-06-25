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
public class UIManager : SingletonBehaviour<UIManager>
{
    // UI프리팹 저장 경로
    const string UiPrefabsPath = "Prefabs";
    const string UiSceneRoot = "@UI_Scene_Root";
    const string UiPopupRoot = "@UI_Popup_Root";

    // 페이지 최대치는 5개로 합니다.
    const int MaxPageOrder = 5;

    // ui render 우선순위
    int pageOrder = 0;
    int popupOrder = 0;

    // UI페이지 관리
    UIPage[] pageAry = new UIPage[MaxPageOrder];

    // UI팝업 관리
    Stack<UIPopup> popupStack = new Stack<UIPopup>();

    public void Init()
    {
        pageAry = null;
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
        GameObject go = Instantiate(prefab.gameObject);
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

        Destroy(ui.gameObject);
        popupOrder--;

        if (popupOrder == 1)
        {   // 마지막 팝업은 삭제할 때 팝업전용 오브젝트도 같이 삭제합니다.
            GameObject popupRoot = Util.CreateObject(UiPopupRoot);
            if (popupRoot == null)
            {
                return;
            }

            Destroy(popupRoot);
        }
    }
    #endregion

    #region PAGE
    // 페이지를 셋팅합니다.
    public void SetPageUI<T>(string name = null) where T : UIPage
    {
        if (name == null)
            name = typeof(T).ToString();

        T prefab = Resources.Load<T>($"{UiPrefabsPath}/{name}");
        if (prefab == null)
            return;

        // 인스턴스를 생성, 씬에 올립니다.
        GameObject go = Instantiate(prefab.gameObject);
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
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = pageOrder;
        pageOrder++;

        // 팝업전용폴더로 옮겨줍니다.
        GameObject popupRoot = Util.CreateObject(UiPopupRoot);
        if (popupRoot == null)
        {
            return;
        }

        go.transform.SetParent(popupRoot.transform);
    }

    // 메인 씬 UI를 
    public void DestroyPageUI<T>() where T : UIPage
    {
        // 파괴할 일이있나?
        Destroy(pageAry[pageOrder].gameObject);
        pageAry[pageOrder] = null;
        pageOrder--;

        GameObject popupRoot = Util.CreateObject(UiPopupRoot);
        if (popupRoot == null)
        {
            return;
        }

        Destroy(popupRoot);
    }
    #endregion
}
