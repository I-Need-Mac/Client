using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���
public class UIPage : UIBase
{
    // �������� ������ �մϴ�.
    public virtual void Open<T>() where T : UIPage
    {
        //UIManager.Instance.SetPageUI<T>(List<object> page);
    }

    // �������� ����ϴ�
    public virtual void Destroy<T>() where T : UIPage
    {
        UIManager.Instance.DestroyPageUI<T>();
    }
}
