using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �˾� ���
public class UIPopup : UIBase
{
    // �˾��� ������ �մϴ�.
    public virtual void Open<T>() where T : UIPopup
    {
        // �Ŵ����� ���� �˾��� ���ϴ�.
        UIManager.Instance.SetPopupUI<T>();
    }

    // �˾��� �ݽ��ϴ�.
    public virtual void Close<T>() where T : UIPopup
    {
        // �Ŵ����� ���� �˾��� ���ϴ�.
        UIManager.Instance.ClosePopupUI<T>();
    }
}
