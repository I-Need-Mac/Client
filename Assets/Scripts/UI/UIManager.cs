using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI를 관리 합니다.
public class UIManager : SingletonBehaviour<UIManager>
{
    // 페이지 최대치는 5개로 합니다.
    const int MaxPageCount = 5;

    // UI페이지 관리
    UIPage[] pages = new UIPage[MaxPageCount];

    // UI팝업 관리
    Stack<UIPopup> popupList = new Stack<UIPopup>();

    // 페이지를 셋팅합니다.
    void SetPageUI()
    {

    }

    // 팝업을 셋팅합니다.
    void SetPopupUI()
    {

    }
}
