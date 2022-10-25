using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI라면 사용하는 모든 기능을 정의합니다.
// 페이지와 팝업은 이 베이스를 상속 받도록 합니다.
public class UIBase : MonoBehaviour
{
    // 이벤트용 UI리스트
    Dictionary<Type, UnityEngine.Object[]> uiList = new Dictionary<Type, UnityEngine.Object[]>();

    // UI이벤트 핸들러를 연결합니다.
    void BindEvent()
    {

    }

    //
}
