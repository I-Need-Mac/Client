using System;
using UnityEngine;
using UnityEngine.EventSystems;

// UI이벤트 핸들러
public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;

    // 오브젝트에서 포인터를 누르고 동일한 오브젝트에서 뗄 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
            if (GetComponent<SoundRequesterBtn>()!= null){
                GetComponent<SoundRequesterBtn>().ChangeSituation(SoundSituation.SOUNDSITUATION.CLICK); 
            }
        }
    }

    // 오브젝트에서 포인터를 ui위에 올렸을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(OnEnterHandler != null)
        {
            OnEnterHandler.Invoke(eventData);
            if (GetComponent<SoundRequesterBtn>() != null)
            {
                GetComponent<SoundRequesterBtn>().ChangeSituation(SoundSituation.SOUNDSITUATION.HOVER);
            }
        }
    }

    // 오브젝트에서 포인터를 ui위에서 내렸을 때 호출 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExitHandler != null)
        {
            OnExitHandler.Invoke(eventData);
            if (GetComponent<SoundRequesterBtn>() != null)
            {
                GetComponent<SoundRequesterBtn>().ChangeSituation(SoundSituation.SOUNDSITUATION.HOVER);
            }
        }
    }
}