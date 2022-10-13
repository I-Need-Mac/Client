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
public class UIManager : MonoSingleton<UIManager>
{
    // UI������ ���� ���
    const string UiPrefabsPath = "Prefabs/UI";
    const string UiPageRoot = "@UI_Page_Root";
    const string UiPopupRoot = "@UI_Popup_Root";

    // ������ �ִ�ġ�� 5���� �մϴ�.
    const int MaxPageOrder = 50;

    // ui render �켱����
    int pageOrder = 0;
    int popupOrder = 0;

    // UI������ ����
    public UIPage[] pageAry = new UIPage[MaxPageOrder];
    public UI_Book[] bookPageAry = new UI_Book[MaxPageOrder];
    public GameObject[] bookPageObj = new GameObject[MaxPageOrder];

    // UI�˾� ����
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

        // �ν��Ͻ��� ����, ���� �ø��ϴ�.
        GameObject go = GameObject.Instantiate(prefab.gameObject);
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

        GameObject.Destroy(ui.gameObject);
        popupOrder--;

        if (popupOrder == 1)
        {   // ������ �˾��� ������ �� �˾����� ������Ʈ�� ���� �����մϴ�.
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
    // �������� �����մϴ�.
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

        // �ν��Ͻ��� ����, ���� �ø��ϴ�.
        GameObject go = GameObject.Instantiate(prefab);
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
        //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //canvas.sortingOrder = pageOrder;
        //pageOrder++;

        // �˾����������� �Ű��ݴϴ�.
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

    // �������� �����մϴ�.
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

        // �ν��Ͻ��� ����, ���� �ø��ϴ�.
        GameObject go = GameObject.Instantiate(prefab);
        if (go == null)
            return;

        bookPageObj[pageOrder] = go;
        go.SetActive(false);

        // T������Ʈ�� �����ɴϴ�.
        T ui = Util.GetOrAddComponent<T>(go);
        if (ui == null)
            return;

        // �������� ����մϴ�.
        bookPageAry[pageOrder] = ui;

        // �켱������ �����մϴ�.
        // ĵ������ �����մϴ�.
        //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //canvas.sortingOrder = pageOrder;
        //pageOrder++;

        // �˾����������� �Ű��ݴϴ�.
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

    // ���� �� UI�� 
    public void DestroyPageUI<T>() where T : UIPage
    {
        // �ı��� �����ֳ�?
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
