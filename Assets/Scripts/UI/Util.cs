using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ��ƿ�� ���� �Լ���
public class Util : MonoBehaviour
{
    // ���� �ڽ��� ã���ϴ�.
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }

        if (recursive == false)
        {   // �������� �� ���� �ڽĸ� ã���ϴ�.
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (transform == null)
                    return null;

                if (transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component == null)
                        return null;

                    return component;
                }
            }
        }
        else
        {   // ���� �ڽ��� ã���ϴ�.
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
        {
            return null;
        }

        return transform.gameObject;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
        {   // ���ٸ� �ٿ��ݴϴ�.
            component = go.AddComponent<T>();
        }

        return component;
    }

    public static GameObject CreateObject(string name)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        // ���� Ȱ������ ���� �����ִ� ������Ʈ�߿� ã���ϴ�.
        GameObject[] objs = activeScene.GetRootGameObjects();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name == name)
            {
                return objs[i];
            }
        }

        // ��ã�� ���� ���� 
        GameObject go = new GameObject { name = name };

        return go;
    }
}
