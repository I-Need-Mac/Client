using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RankBoard : UI_Popup
{
    enum Images
    {
        Close,
    }
    [SerializeField]
    TextMeshProUGUI titleText;
    [Header("-----RankData-----")]
    [SerializeField]
    TextMeshProUGUI rankText;
    [SerializeField]
    TextMeshProUGUI nicknameText;
    [SerializeField]
    TextMeshProUGUI progressText;
    [SerializeField]
    TextMeshProUGUI characterText;
    [SerializeField]
    TextMeshProUGUI timeText;
    [Header("-----PlayerInfo-----")]
    [SerializeField]
    TextMeshProUGUI playernicknameText;
    [SerializeField]
    TextMeshProUGUI cleartimeText;
    [SerializeField]
    TextMeshProUGUI playercleartimeText;
    [SerializeField]
    TextMeshProUGUI levelText;
    private void Start()
    {
        Bind<Image>(typeof(Images));

        Array imageValue = Enum.GetValues(typeof(Images));

        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }
        titleText.text = LocalizeManager.Instance.GetText("UI_RankingBoard");
        cleartimeText.text = LocalizeManager.Instance.GetText("UI_ClearTime_Text");
        playercleartimeText.text = LocalizeManager.Instance.GetText("UI_ClearTime");
        rankText.text = LocalizeManager.Instance.GetText("UI_Rank_Rank");
        nicknameText.text = LocalizeManager.Instance.GetText("UI_Rank_Name");
        progressText.text = LocalizeManager.Instance.GetText("UI_Rank_Progress");
        characterText.text = LocalizeManager.Instance.GetText("UI_Rank_Character");
        timeText.text = LocalizeManager.Instance.GetText("UI_Rank_Time");
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.Close:
                this.CloseUI<UI_RankBoard>();
                break;
            default:
                break;
        }
    }
}
