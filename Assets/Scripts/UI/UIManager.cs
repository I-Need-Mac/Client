using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI�� ���� �մϴ�.
public class UIManager : SingletonBehaviour<UIManager>
{
    // ������ �ִ�ġ�� 5���� �մϴ�.
    const int MaxPageCount = 5;

    // UI������ ����
    UIPage[] pages = new UIPage[MaxPageCount];

    // UI�˾� ����
    Stack<UIPopup> popupList = new Stack<UIPopup>();

    // �������� �����մϴ�.
    void SetPageUI()
    {

    }

    // �˾��� �����մϴ�.
    void SetPopupUI()
    {

    }
}
