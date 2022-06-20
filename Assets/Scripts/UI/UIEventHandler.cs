using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI이벤트 핸들러
public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData> OnDownHandler = null;
    public Action<PointerEventData> OnUpHandler = null;

    // 오브젝트에서 포인터를 누르고 동일한 오브젝트에서 뗄 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    // 포인터가 오브젝트에 들어왔을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnterHandler != null)
        {
            OnEnterHandler.Invoke(eventData);
        }
    }

    // 포인터가 오브젝트로부터 멀어졌을 때에 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExitHandler != null)
        {
            OnEnterHandler.Invoke(eventData);
        }
    }

    // 포인터가 오브젝트를 눌렀을 때 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnDownHandler != null)
        {
            OnDownHandler.Invoke(eventData);
        }
    }

    // 포인터를 뗐을 때 호출(눌려 있는 오브젝트에서 호출)
    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnUpHandler != null)
        {
            OnUpHandler.Invoke(eventData);
        }
    }
}