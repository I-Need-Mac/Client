using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Sprite Origin;
    [SerializeField]
    Sprite Hover;
    [SerializeField]
    Sprite Click;

    [SerializeField]
    Image  img;
    private void Start()
    {
        if( img==null) img =GetComponent<Image>();

        EventTrigger trigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

        // Pointer Enter 이벤트 추가
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entryEnter);

        // Pointer Exit 이벤트 추가
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entryExit);

        EventTrigger.Entry entryClick = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerClick;
        entryExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entryExit);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.sprite = Hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.sprite = Origin;
        // 마우스가 이미지에서 벗어났을 때 실행할 코드
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        img.sprite = Click;
    }
}
