using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �����Ұ�
 * 1. UI�� �ø������� ���ҽ��� �ε��ϴ°��� �δ�Ǵ��� �����غ���
 * ���� �δ��� �ȴٸ� ���� ���۽� �̸� ���ҽ��� �ε��صΰ� ������ �� �ε��ص� ���ҽ��� �������Ѵ�.
 * 2. UI���� ĵ������ �پ��ִ°� ����
 * ���� �ڵ�δ� UI���� ĵ������ �� �پ��־�� �ϹǷ� �ϳ��� ĵ������ ������ �ٿ��� �� �� �ֵ��� �����ʿ�
 * 3. �˾� �ڵ� ��� ���� �ʿ� (PC�� �°� ����)
 * 
 * �߰��Ұ�
 * �˾� ��� �з��ؾ��� (��ü �˾� / �κ� �˾� / Message�˾� ��
 */

// UI�� ���� �մϴ�.
public class UIManager : SingletonBehaviour<UIManager>
{
    // UI������ ���� ���
    const string UiPrefabsPath = "Prefabs";
    const string UiSceneRoot = "@UI_Scene_Root";
    const string UiPopupRoot = "@UI_Popup_Root";

    // ������ �ִ�ġ�� 5���� �մϴ�.
    const int MaxPageOrder = 5;

    // ui render �켱����
    int pageOrder = 0;
    int popupOrder = 0;

    // UI������ ����
    UIPage[] pageAry = new UIPage[MaxPageOrder];

    // UI�˾� ����
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

        // �ν��Ͻ��� ����, ���� �ø��ϴ�.
        GameObject go = Instantiate(prefab.gameObject);
        if (go == null)
            return;

        // T������Ʈ�� �����ɴϴ�.
        T ui = Util.GetOrAddComponent<T>(go);
        if (ui == null)
            return;

        // ���ÿ� �߰��մϴ�.
        popupStack.Push(ui);

        // �켱������ �����մϴ�.
        // ĵ������ �����մϴ�.
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = popupOrder;
        popupOrder++;

        // �˾����������� �Ű��ݴϴ�.
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
        {   // ������ �˾��� ������ �� �˾����� ������Ʈ�� ���� �����մϴ�.
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
    // �������� �����մϴ�.
    public void SetPageUI<T>(string name = null) where T : UIPage
    {
        if (name == null)
            name = typeof(T).ToString();

        T prefab = Resources.Load<T>($"{UiPrefabsPath}/{name}");
        if (prefab == null)
            return;

        // �ν��Ͻ��� ����, ���� �ø��ϴ�.
        GameObject go = Instantiate(prefab.gameObject);
        if (go == null)
            return;

        // T������Ʈ�� �����ɴϴ�.
        T ui = Util.GetOrAddComponent<T>(go);
        if (ui == null)
            return;

        // �������� ����մϴ�.
        pageAry[pageOrder] = ui;

        // �켱������ �����մϴ�.
        // ĵ������ �����մϴ�.
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = pageOrder;
        pageOrder++;

        // �˾����������� �Ű��ݴϴ�.
        GameObject popupRoot = Util.CreateObject(UiPopupRoot);
        if (popupRoot == null)
        {
            return;
        }

        go.transform.SetParent(popupRoot.transform);
    }

    // ���� �� UI�� 
    public void DestroyPageUI<T>() where T : UIPage
    {
        // �ı��� �����ֳ�?
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
