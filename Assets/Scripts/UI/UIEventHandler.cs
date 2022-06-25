using System;
using UnityEngine;
using UnityEngine.EventSystems;

// UI�̺�Ʈ �ڵ鷯
public class UIEventHandler : MonoBehaviour, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;

    // ������Ʈ���� �����͸� ������ ������ ������Ʈ���� �� �� ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        // �켱 Ŭ�� �̺�Ʈ�� ó���ϵ��� �մϴ�.
        // ���� mouse over, up, down�� �̺�Ʈ�� �߰��ϵ��� �մϴ�.
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    // Ű���� �̺�Ʈ�� �߰��� �� �ֽ��ϴ�.
}