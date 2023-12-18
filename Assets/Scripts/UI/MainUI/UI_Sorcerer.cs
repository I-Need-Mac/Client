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
        Image,
        Ultimate
    }

    enum Texts
    {
        Name
    }

    [SerializeField]
    Image selectImage;
    [SerializeField]
    TextMeshProUGUI sorcererName;
    [SerializeField]
    Image ultiImage;

    int sorcererInfoID = 0;
    private bool sorcererUnlock;
    Dictionary<string, object> sorcererInfo = new Dictionary<string, object>();

    void Start()
    {
        CharacterUnlock();
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length-1; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        Bind<Text>(typeof(Texts));
        Array textValue = Enum.GetValues(typeof(Texts));
        for (int i = 0; i < textValue.Length-1; i++)
        {
            BindUIEvent(GetText(i).gameObject, (PointerEventData data) => { OnClickText(data); }, Define.UIEvent.Click);
        }
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.Image:
                if(sorcererUnlock)
                {
                     UI_SelectSorcererInfo info = UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                     info.SetSorcererInfo(sorcererInfoID, sorcererInfo);
                }
                else
                {
                    UIManager.Instance.OpenUI<UI_Hon_Unlock_conditions>();
                }
                break;
            case Images.Ultimate:
                break;
            default:
                break;
        }
    }

    public void OnClickText(PointerEventData data)
    {
        Texts textValue = (Texts)FindEnumValue<Texts>(data.pointerClick.name);
        if ((int)textValue < -1)
            return;
        
        Debug.Log(data.pointerClick.name);

        switch (textValue)
        {
            case Texts.Name:
                UI_SelectSorcererInfo info = UIManager.Instance.OpenUI<UI_SelectSorcererInfo>();
                info.SetSorcererInfo(sorcererInfoID, sorcererInfo);
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

        string ultiPath = GetUltiPath(ultimate);
        Sprite ultiSprite = Resources.Load<Sprite>($"{Define.UiArtsPath}/" + ultiPath);
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
            else if (val.Key == UIData.CharacterTableCol.MainShowImagePath.ToString())
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
        foreach (KeyValuePair<string, Dictionary<string, object>> data in skillData)
        {
            if(data.Key == skillID)
            {
                foreach (KeyValuePair<string, object> val in data.Value)
                {
                    if (val.Key == "Icon")
                    {
                        ultiPath = val.Value.ToString();
                        break;
                    }
                }

                break;
            }    
        }

        return ultiPath;
    }
    async void CharacterUnlock()
    {
        if (!SteamManager.Initialized) { return; }
        string name = SteamUser.GetSteamID().ToString();     
        switch (sorcererInfoID)
        {
            case 101:
                sorcererUnlock = await APIManager.Instance.CheckCharacterUnlock(name, "adf", "hojin");
                break;
            case 102:
                sorcererUnlock = await APIManager.Instance.CheckCharacterUnlock(name, "adf", "siwoo");
                break;
            case 103:
                sorcererUnlock = await APIManager.Instance.CheckCharacterUnlock(name, "adf", "sinwol");
                break;
            case 104:
                sorcererUnlock = await APIManager.Instance.CheckCharacterUnlock(name, "adf", "ulises");
                break;
            case 105:
                sorcererUnlock = await APIManager.Instance.CheckCharacterUnlock(name, "adf", "seimei");
                break;
            case 106:
                sorcererUnlock = await APIManager.Instance.CheckCharacterUnlock(name, "adf", "macia");
                break;
        }
    }
}