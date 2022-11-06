using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// UI�� ���� �մϴ�. (Create/Open/Close)
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

    // ���� UI �켱����
    int mainUiOrder = 0;
    // �˾�UI �켱����
    int currentPopupCount = 0;

    // ���� UI
    UI_StartMain mainUI;
    // UI��ü �˾� ���
    List<UI_Popup> popupList = new List<UI_Popup>();

    // �ǽð� �˾� ���
    LinkedList<UI_Popup> currentPopup = new LinkedList<UI_Popup>();

    public void Init()
    {
        // UI���� ���̺��� �н��ϴ�.
        UIData.ReadData();

        // �̺�Ʈ �ý����� �߰��մϴ�.
        GameObject go = GameObject.Find("EventSystem");
        if( go == null )
        {   // �̺�Ʈ �ý����� ���ٸ� �ϳ� �����մϴ�.
            go = new GameObject("EventSystem");
            Util.GetOrAddComponent<EventSystem>(go);
            Util.GetOrAddComponent<StandaloneInputModule>(go);
        }

        // ���� �ʱ�ȭ
        popupList.Clear();

        // ui�� �����մϴ�.
        Create();

        // ���� UI�� �׻� �ֻ����� �׷����ϴ�.
        mainUiOrder = 0;
        // �ʱ� �켱���� ����
        currentPopupCount = 0;
    }

    // UI�� �����մϴ�.
    public void Create()
    {
        string[] names = Enum.GetNames(typeof(UI_Prefab));

        // ���� UI�� �����մϴ�.
        mainUI = Util.UILoad<UI_StartMain>($"{Define.UiPrefabsPath}/{names[(int)UI_Prefab.UI_StartMain]}");
        if (mainUI == null)
        {
            Debug.Log("mainUI is NULL");
            return;
        }

        // ĵ������ �����մϴ�.
        SetCanvas(mainUI.gameObject);
        Util.CreateObject(mainUI.gameObject);

        // ��ü UI����Ʈ�� �����մϴ�.
        for ( int i = 0; i < names.Length; i++ )
        {
            if (i == (int)UI_Prefab.UI_StartMain)
                continue;

            // �˾� UI�� �����մϴ�.
            UI_Popup popup = Util.UILoad<UI_Popup>($"{Define.UiPrefabsPath}/{names[i]}");
            if ( popup == null )
            {
                Debug.Log(names[i] + " is NULL");
                continue;
            }

            // ĵ������ �����մϴ�.
            SetCanvas(popup.gameObject);

            // Ȱ����Ű�� ���� ���·� �ʱ�ȭ �մϴ�.
            popup.gameObject.SetActive(false);

            // ����Ʈ�� �߰��մϴ�.
            popupList.Add(popup);
        }
    }

    // ĵ������ �����մϴ�.
    private void SetCanvas(GameObject go)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // �켱������ 0���� �ʱ�ȭ �մϴ�.
        canvas.sortingOrder = 0;

        CanvasScaler canvasScaler = Util.GetOrAddComponent<CanvasScaler>(go);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(Define.uiScreenWidth, Define.uiScreenHeight);

        Util.GetOrAddComponent<GraphicRaycaster>(go);
    }

    // ui�� ���ϴ�
    public T OpenUI<T>() where T : UI_Base
    {
        UI_Popup popup = FindPopupUI(typeof(T).Name);

        // �̹� �������ִ� �˾� �Դϴ�.
        if (IsCurrentPopup(popup))
            return null;
        
        // ������Ʈ�� �����մϴ�.
        GameObject go = Util.CreateObject(popup.gameObject);

        // �켱���� ����
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = mainUiOrder + 1;
        
        // ���� �������� �˾� �� ����
        currentPopupCount++;

        go.SetActive(true);

        // �ǽð� ui����Ʈ�� �߰��մϴ�.
        currentPopup.AddFirst(popup);

        // �˾����������� �Ű��ݴϴ�.
        GameObject popupRoot = Util.GetOrCreateObjectInActiveScene(Define.UiPopupRoot);
        if (popupRoot == null)
        {
            return null;
        }
        go.transform.SetParent(popupRoot.transform);

        return go.GetComponent<T>();
    }

    // ui�� �ݽ��ϴ�
    public void CloseUI<T>() where T : UI_Base
    {
        // ���� �˾��� �����ϴ�.
        if (currentPopup.Count <= 0)
            return;

        // ����Ʈ���� ����
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

        // ������Ʈ ����
        GameObject popupRoot = Util.GetOrCreateObjectInActiveScene(Define.UiPopupRoot);
        GameObject findObject = Util.FindChild(popupRoot, typeof(T).Name);
        Destroy(findObject);

        currentPopupCount--;
    }

    private UI_Popup FindPopupUI(string name)
    {
        // �˾� ����Ʈ���� �ش��ϴ� �˾��� ã���ϴ�.
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
