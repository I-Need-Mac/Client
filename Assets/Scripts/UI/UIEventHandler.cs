using System;
using UnityEngine;
using UnityEngine.EventSystems;

// UI이벤트 핸들러
public class UIEventHandler : MonoBehaviour, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;

    // 오브젝트에서 포인터를 누르고 동일한 오브젝트에서 뗄 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        // 우선 클릭 이벤트만 처리하도록 합니다.
        // 추후 mouse over, up, down등 이벤트를 추가하도록 합니다.
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
            GetComponent<SoundRequesterBtn>().ChangeSituation(SoundSituation.SOUNDSITUATION.CLICK);
        }
    }

}