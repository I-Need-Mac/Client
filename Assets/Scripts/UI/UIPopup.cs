using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 팝업 기능
public class UIPopup : UIBase
{
    // 팝업을 열도록 합니다.
    public virtual void Open<T>() where T : UIPopup
    {
        // 매니저를 통해 팝업을 엽니다.
        UIManager.Instance.SetPopupUI<T>();
    }

    // 팝업을 닫습니다.
    public virtual void Close<T>() where T : UIPopup
    {
        // 매니저를 통해 팝업을 엽니다.
        UIManager.Instance.ClosePopupUI<T>();
    }
}
