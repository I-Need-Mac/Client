using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 유틸로 쓰는 함수들
public class Util : MonoBehaviour
{
    public static T Load<T>(string path) where T : UI_Base
    {
        return Resources.Load<T>(path);
    }

    public static GameObject CreateObject(GameObject go)
    {
        GameObject createObject = GameObject.Instantiate(go);
        string reName = createObject.name.Replace("(Clone)", "").Trim();
        createObject.name = reName;

        return createObject;
    }

    // 하위 자식을 찾습니다.
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }

        if (recursive == false)
        {   // 직속으로 내 하위 자식만 찾습니다.
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
        {   // 하위 자식을 찾습니다.
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
        {   // 없다면 붙여줍니다.
            component = go.AddComponent<T>();
        }

        return component;
    }

    public static GameObject GetOrCreateObjectInActiveScene(string name)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        // 현재 활성중인 씬에 속해있는 오브젝트중에 찾습니다.
        GameObject[] objs = activeScene.GetRootGameObjects();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name == name)
            {
                return objs[i];
            }
        }

        // 못찾음 새로 만들어서 
        GameObject go = new GameObject { name = name };

        return go;
    }
}
