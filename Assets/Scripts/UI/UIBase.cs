using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// UI��� ����ϴ� ��� ����� �����մϴ�.
// �������� �˾��� �� ���̽��� ��� �޵��� �մϴ�.
public class UIBase : MonoBehaviour
{
    // �̺�Ʈ�� UI����Ʈ
    Dictionary<Type, UnityEngine.Object[]> uiList = new Dictionary<Type, UnityEngine.Object[]>();

    // UI�̺�Ʈ �ڵ鷯�� �����մϴ�.
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

    // UI��Ҹ� ���ε� �մϴ�.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // enum�� �ִ� ����Ʈ���� string���� �̾ƿ� �� ����.
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
        // Ÿ�Կ� ���� ����Ʈ�� �����´�
        UnityEngine.Object[] objects = null;
        if (uiList.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        // ������ ����Ʈ�� �´� �ε��� ��Ҹ� T�� ĳ�����ؼ� ��ȯ
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
