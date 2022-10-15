using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// UI라면 사용하는 모든 기능을 정의합니다.
// 페이지와 팝업은 이 베이스를 상속 받도록 합니다.
public class UI_Base : MonoBehaviour
{
    public T CreateUI<T>(string name = null) where T : UI_Base
    {   // UI를 생성합니다.
        if (name == null)
            name = typeof(T).ToString();

        // UI의 프리팹을 로드합니다.
        T prefab = Resources.Load<T>($"{Define.UiPrefabsPath}/{name}");
        if (prefab == null)
            return null;

        return prefab;
    }

    // 팝업을 열도록 합니다.
    public virtual void OpenUI<T>() where T : UI_Base
    {
        // 매니저를 통해 팝업을 엽니다.
        UIManager.Instance.OpenUI<T>();
    }

    // 팝업을 닫습니다.
    public virtual void CloseUI<T>() where T : UI_Base
    {
        // 매니저를 통해 팝업을 엽니다.
        UIManager.Instance.CloseUI<T>();
    }

    // 이벤트용 UI리스트
    Dictionary<Type, UnityEngine.Object[]> uiList = new Dictionary<Type, UnityEngine.Object[]>();

    // UI이벤트 핸들러를 연결합니다.
    public void BindUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.None)
    {
        UIEventHandler evt = Util.GetOrAddComponent<UIEventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            default:
                break;
        } 
    }

    // UI요소를 바인딩 합니다.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // enum에 있는 리스트들을 string으로 뽑아올 수 있음.
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        uiList.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.Log($"Faild to bind ({names[i]})");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        // 타입에 대한 리스트를 가져온다
        UnityEngine.Object[] objects = null;
        if (uiList.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        // 가져온 리스트에 맞는 인덱스 요소를 T로 캐스팅해서 반환
        return objects[idx] as T;
    }

    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected int FindEnumValue<T>(string name)
    {
        if (name == null)
        {
            return -1;
        }

        string[] enumStr = Enum.GetNames(typeof(T));
        for (int i = 0; i < enumStr.Length; i++)
        {
            if (enumStr[i] == name)
            {
                return i;
            }
        }

        return -1;
    }

}
