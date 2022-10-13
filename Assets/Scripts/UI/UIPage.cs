using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 페이지 기능
public class UIPage : UIBase
{
    // 페이지를 열도록 합니다.
    public virtual void Open<T>() where T : UIPage
    {
        //UIManager.Instance.SetPageUI<T>(List<object> page);
    }

    // 페이지를 지웁니다
    public virtual void Destroy<T>() where T : UIPage
    {
        UIManager.Instance.DestroyPageUI<T>();
    }
}
