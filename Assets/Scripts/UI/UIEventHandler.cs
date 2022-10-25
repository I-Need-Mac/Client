using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI�̺�Ʈ �ڵ鷯
public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData> OnDownHandler = null;
    public Action<PointerEventData> OnUpHandler = null;

    // ������Ʈ���� �����͸� ������ ������ ������Ʈ���� �� �� ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    // �����Ͱ� ������Ʈ�� ������ �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnterHandler != null)
        {
            OnEnterHandler.Invoke(eventData);
        }
    }

    // �����Ͱ� ������Ʈ�κ��� �־����� ���� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExitHandler != null)
        {
            OnEnterHandler.Invoke(eventData);
        }
    }

    // �����Ͱ� ������Ʈ�� ������ �� ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnDownHandler != null)
        {
            OnDownHandler.Invoke(eventData);
        }
    }

    // �����͸� ���� �� ȣ��(���� �ִ� ������Ʈ���� ȣ��)
    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnUpHandler != null)
        {
            OnUpHandler.Invoke(eventData);
        }
    }
}