using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Sorcerer : UI_Base
{
    enum Images
    {
        TouchPanel,
        ImageLock
    }


    [SerializeField]
    Image selectImage;
    [SerializeField]
    TextMeshProUGUI sorcererName;
    [SerializeField]
    Image ultiImage;
    [SerializeField]
    GameObject selected;
    [SerializeField]
    GameObject locked;

    public int sorcererInfoID = 0;
    private bool sorcererUnlock;
    Dictionary<string, object> sorcererInfo = new Dictionary<string, object>();

    void Start()
    {
      
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }


        CharacterUnlock();
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log("[Click] Sorcerer "+data.pointerClick.name);

        switch (imageValue)
        {
            case Images.TouchPanel:
                 UI_SelectSorcererInfo info = UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                 info.SetSorcererInfo(sorcererInfoID, sorcererInfo);
            break;

            case Images.ImageLock:
                UI_Sorcerer_Unlock_conditions buy =UIManager.Instance.OpenUI<UI_Sorcerer_Unlock_conditions>();
                buy.SetSorcererName(sorcererInfo[UIData.CharacterTableCol.CharacterName.ToString()].ToString(), sorcererInfoID);
                break;
            default:
                break;
        }
    }



    public void SetResource(string imagePath, string name, string ultimate)
    {
        sorcererName.text = name;

        Sprite imageSprite = Resources.Load<Sprite>($"{Define.UiArtsPath}/" + imagePath);
        if(imageSprite == null)
            return;
        selectImage.sprite = imageSprite;

        ultimate = GetUltiPath(ultimate);
        Sprite ultiSprite = Resources.Load<Sprite>( ultimate);

        if (ultiSprite == null)
            return;
        ultiImage.sprite = ultiSprite;
    }

    public void SetSorcerer(int id, Dictionary<string, object> sorcerer)
    {
        sorcererInfoID = id;

        string imagePath = "";
        string name = "";
        string ultiIconPath = "";

        foreach (KeyValuePair<string, object> val in sorcerer)
        {
            sorcererInfo.Add(val.Key, val.Value);
            
            if (val.Key == UIData.CharacterTableCol.CharacterName.ToString())
            {
                name = LocalizeManager.Instance.GetText(val.Value.ToString());
            }
            else if (val.Key == UIData.CharacterTableCol.MainSelectImagePath.ToString())
            {
                imagePath = val.Value.ToString();
            }
            else if (val.Key == UIData.CharacterTableCol.SkillID_01.ToString())
            {
                ultiIconPath = val.Value.ToString();
            }
        }

        SetResource(imagePath, name, ultiIconPath);
    }

    string GetUltiPath(string skillID)
    {   // 스킬 id로 궁극기 아이콘 path를 가져옵니다.
        string ultiPath = "";

        Dictionary<string, Dictionary<string, object>> skillData = UIData.SkillData;
        ultiPath = skillData[skillID]["Icon"].ToString();

        return ultiPath;
    }
    public void CharacterUnlock()
    {
        switch (sorcererInfoID)
        {
            case (int)UIStatus.Sorcerers.hojin :
                sorcererUnlock = UIStatus.Instance.hojin;
                SetIsLocked(sorcererUnlock);
                break;
            case (int)UIStatus.Sorcerers.siWoo:
                sorcererUnlock = UIStatus.Instance.siWoo;
                SetIsLocked(sorcererUnlock);
                break;
            case (int)UIStatus.Sorcerers.sinwol:
                sorcererUnlock = UIStatus.Instance.sinwol;
                SetIsLocked(sorcererUnlock);
                break;
            case (int)UIStatus.Sorcerers.ulises:
                sorcererUnlock = UIStatus.Instance.ulises;
                SetIsLocked(sorcererUnlock);
                break;
            case (int)UIStatus.Sorcerers.seimei:
                sorcererUnlock = UIStatus.Instance.seimei;
                SetIsLocked(sorcererUnlock);
                break;
            case (int)UIStatus.Sorcerers.macia:
                sorcererUnlock = UIStatus.Instance.macia;
                SetIsLocked(sorcererUnlock);
                break;
        }
    }
    public void SetIsSelected(bool isActive) {
        selected.SetActive(isActive);
    }

    public void SetIsSelected()
    {
        if (UIStatus.Instance.selectedChar == sorcererInfoID) {
            selected.SetActive(true);
        }
        
    }
    public void SetIsLocked(bool isLock)
    {
        locked.SetActive(!isLock);
    }
}