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

    private bool access;
    private bool isSelect;

    [SerializeField]
    private TMP_FontAsset[] font;
    [SerializeField]
    private Image[] selectImage;

    public Image image;
    private void Start()
    {
        SetStageID();
        UIManager.Instance.stageList.Add(this);
        prefabIndex = transform.GetSiblingIndex();
        Bind<GameObject>(typeof(GameObjects));
        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }
        stageId = int.Parse(stageIds[prefabIndex]);
        if (UIStatus.Instance.last_stage == 0 && stageId % 10 == 1)
        {
            access = true;
        }
        else if (stageId % 10 <= UIStatus.Instance.last_stage % 10 + 1)
        {
            access = true;
        }
        else
        {
            access = false;
            Color textColor = text.color;
            textColor.a = 0.5f;
            text.color = textColor;
        }
        //access = true;        
        if(prefabIndex == 0)
        {
            isSelect = true;
            selectImage[0].gameObject.SetActive(false);
            selectImage[1].gameObject.SetActive(true);
            text.font = font[1];
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
                if (access)
                {
                    UIManager.Instance.selectStageID = stageId;
                    OnSelectImage();
                }
                else
                {
                    Debug.Log("아직 접근할 수 없습니다");
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

        foreach (var entry in stageTable)
        {
            string stageId = entry.Key;
            stageIds.Add(stageId);
        }
    }
    public void OnSelectImage()
    {
        foreach (var stage in UIManager.Instance.stageList)
        {
            if (stage == this)
            {
                stage.isSelect = (stageId == UIManager.Instance.selectStageID);

                if (stage.selectImage[0] != null && stage.selectImage[1] != null)
                {
                    stage.selectImage[0].gameObject.SetActive(!stage.isSelect);
                    stage.selectImage[1].gameObject.SetActive(stage.isSelect);
                }

                if (stage.text != null)
                {
                    stage.text.font = (stage.isSelect) ? font[1] : font[0];
                }
            }
            else
            {
                stage.isSelect = false;

                if (stage.selectImage[0] != null && stage.selectImage[1] != null)
                {
                    stage.selectImage[0].gameObject.SetActive(!stage.isSelect);
                    stage.selectImage[1].gameObject.SetActive(stage.isSelect);
                }

                if (stage.text != null)
                {
                    stage.text.font = font[0];
                }
            }
        }
    }
}
