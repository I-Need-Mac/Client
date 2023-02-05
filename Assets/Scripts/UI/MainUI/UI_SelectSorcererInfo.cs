using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectSorcererInfo : UI_Popup
{
    enum Images
    {
        BackBtn
    }

    enum Buttons
    {
        StoryBtn,
        StatBtn,
        SkillBtn,

        Select,
    }

    [SerializeField]
    Text titleText;
    [SerializeField]
    Text storyText;
    [SerializeField]
    Text statText;
    [SerializeField]
    Text skillText;

    [SerializeField]
    Text sorcererName;
    [SerializeField]
    Image selectImage;

    [SerializeField]
    GameObject StoryContents;
    [SerializeField]
    Text storyTopText;
    [SerializeField]
    Text storyMiddleText;
    [SerializeField]
    Text storyBottomText;

    [SerializeField]
    GameObject StatContents;
    
    [SerializeField]
    GameObject SkillContents;
    [SerializeField]
    Image Skill_1_Image;
    [SerializeField]
    Text Skill_1_Name;
    [SerializeField]
    Text Skill_1_Context;
    [SerializeField]
    Image Skill_2_Image;
    [SerializeField]
    Text Skill_2_Name;
    [SerializeField]
    Text Skill_2_Context;

    int sorcererInfoID = 0;
    Dictionary<string, object> sorcererInfo = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        Bind<Image>(typeof(Images));
        Array imageValue = Enum.GetValues(typeof(Images));
        for (int i = 0; i < imageValue.Length; i++)
        {
            BindUIEvent(GetImage(i).gameObject, (PointerEventData data) => { OnClickImage(data); }, Define.UIEvent.Click);
        }

        Bind<Button>(typeof(Buttons));
        Array buttonValue = Enum.GetValues(typeof(Buttons));
        for (int i = 0; i < buttonValue.Length; i++)
        {
            BindUIEvent(GetButton(i).gameObject, (PointerEventData data) => { OnClickButton(data); }, Define.UIEvent.Click);
        }

        StoryContents.gameObject.SetActive(true);
        StatContents.gameObject.SetActive(false);
        SkillContents.gameObject.SetActive(false);

        titleText.text = LocalizeManager.Instance.GetText("UI_Sorcer");
        storyText.text = LocalizeManager.Instance.GetText("UI_Story");
        statText.text = LocalizeManager.Instance.GetText("UI_Stat");
        skillText.text = LocalizeManager.Instance.GetText("UI_Skill");
    }

    public void OnClickImage(PointerEventData data)
    {
        Images imageValue = (Images)FindEnumValue<Images>(data.pointerClick.name);
        if ((int)imageValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (imageValue)
        {
            case Images.BackBtn:
                UIManager.Instance.CloseUI<UI_SelectSorcererInfo>();
                break;
            default:
                break;
        }
    }

    public void OnClickButton(PointerEventData data)
    {
        Buttons buttonValue = (Buttons)FindEnumValue<Buttons>(data.pointerClick.name);
        if ((int)buttonValue < -1)
            return;

        Debug.Log(data.pointerClick.name);

        switch (buttonValue)
        {
            case Buttons.StoryBtn:
                StoryContents.gameObject.SetActive(true);
                StatContents.gameObject.SetActive(false);
                SkillContents.gameObject.SetActive(false);
                break;
            case Buttons.StatBtn:
                StoryContents.gameObject.SetActive(false);
                StatContents.gameObject.SetActive(true);
                SkillContents.gameObject.SetActive(false);
                break;
            case Buttons.SkillBtn:
                StoryContents.gameObject.SetActive(false);
                StatContents.gameObject.SetActive(false);
                SkillContents.gameObject.SetActive(true);
                break;
            case Buttons.Select:
                UIManager.Instance.selectCharacterID = sorcererInfoID;
                CloseUI<UI_SelectSorcererInfo>();
                break;
            default:
                break;
        }
    }

    public void SetSorcererInfo(int id, Dictionary<string, object> sorcerer)
    {
        sorcererInfoID = id;

        string skill_1;
        string skill_2;

        foreach (KeyValuePair<string, object> val in sorcerer)
        {
            sorcererInfo.Add(val.Key, val.Value);

            if (val.Key == UIData.CharacterTableCol.CharacterName.ToString())
            {
                sorcererName.text = val.Value.ToString();
            }
            else if (val.Key == UIData.CharacterTableCol.MainShowImagePath.ToString())
            {
                Sprite imageSprite = Resources.Load<Sprite>($"{Define.UiArtsPath}/" + val.Value.ToString());
                if (imageSprite == null)
                    return;
                selectImage.sprite = imageSprite;

            }
            else if(val.Key == UIData.CharacterTableCol.SkillID_01.ToString())
            {
                Skill_1_Name.text = GetSkillName(val.Value.ToString());
                Skill_1_Image.sprite = GetSkillSprite(val.Value.ToString());
                Skill_1_Context.text = GetSkillDesc(val.Value.ToString());
            }
            else if (val.Key == UIData.CharacterTableCol.SkillID_02.ToString())
            {
                Skill_2_Name.text = GetSkillName(val.Value.ToString());
                Skill_2_Image.sprite = GetSkillSprite(val.Value.ToString());
                Skill_2_Context.text = GetSkillDesc(val.Value.ToString());
            }
        }
    }

    string GetSkillName(string skillID)
    {
        string skillName = "";

        Dictionary<string, Dictionary<string, object>> skillData = UIData.SkillData;
        foreach (KeyValuePair<string, Dictionary<string, object>> data in skillData)
        {
            if (data.Key == skillID)
            {
                foreach (KeyValuePair<string, object> val in data.Value)
                {
                    if (val.Key == UIData.SkillTableCol.Name.ToString())
                    {
                        skillName = val.Value.ToString();
                        break;
                    }
                }

                break;
            }
        }

        return skillName;
    }

    Sprite GetSkillSprite(string skillID)
    {
        Sprite skillSprite = null;

        Dictionary<string, Dictionary<string, object>> skillData = UIData.SkillData;
        foreach (KeyValuePair<string, Dictionary<string, object>> data in skillData)
        {
            if (data.Key == skillID)
            {
                foreach (KeyValuePair<string, object> val in data.Value)
                {
                    if (val.Key == UIData.SkillTableCol.SkillImage.ToString())
                    {
                        skillSprite = Resources.Load<Sprite>($"{Define.UiArtsPath}/" + val.Value.ToString());
                        break;
                    }
                }

                break;
            }
        }

        return skillSprite;
    }

    string GetSkillDesc(string skillID)
    {
        string skillDesc = "";

        Dictionary<string, Dictionary<string, object>> skillData = UIData.SkillData;
        foreach (KeyValuePair<string, Dictionary<string, object>> data in skillData)
        {
            if (data.Key == skillID)
            {
                foreach (KeyValuePair<string, object> val in data.Value)
                {
                    if (val.Key == UIData.SkillTableCol.Desc.ToString())
                    {
                        skillDesc = val.Value.ToString();
                        break;
                    }
                }

                break;
            }
        }

        return skillDesc;
    }
}
