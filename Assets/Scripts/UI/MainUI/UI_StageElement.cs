using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StageElement : UI_Base
{
    enum GameObjects
    {
        Text
    }
    public TextMeshProUGUI text;
    public int stageId;
    int prefabIndex;
    List<string> stageIds;
    public bool access;
    private void Start()
    {
        SetStageID();
        prefabIndex = transform.GetSiblingIndex();
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
        
        stageId = int.Parse(stageIds[prefabIndex]);
        if(UIStatus.Instance.last_stage == 0 && stageId%10 == 1)
        {
            access= true;
        }
        else if(stageId%10<=UIStatus.Instance.last_stage %10+1)
        {
            access = true;
        }
        else
        {
            access = false;
        }
    }
    public void OnClickObject(PointerEventData data)
    {
        GameObjects imageValue = (GameObjects)FindEnumValue<GameObjects>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case GameObjects.Text:
                if(access)
                {
                    UIManager.Instance.selectStageID= stageId;
                    Debug.Log("SelectStage :"+UIManager.Instance.selectStageID);
                }
                else
                {
                    Debug.LogError("아직 접근할 수 없습니다");
                }
                break;
            default:
                break;
        }
    }
    public void SetStageID()
    {
        Dictionary<string, Dictionary<string, object>> stageTable = CSVReader.Read("StageTable");
        stageIds = new List<string>();

        // 스테이지 ID를 읽어와서 리스트에 추가합니다.
        foreach (var entry in stageTable)
        {
            string stageId = entry.Key;
            stageIds.Add(stageId);
        }
    }

}
