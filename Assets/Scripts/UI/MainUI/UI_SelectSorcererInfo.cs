using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    TextMeshProUGUI titleText;
    [SerializeField]
    TextMeshProUGUI storyText;
    [SerializeField]
    TextMeshProUGUI statText;
    [SerializeField]
    TextMeshProUGUI skillText;

    [SerializeField]
    TextMeshProUGUI sorcererName;
    [SerializeField]
    Image selectImage;

    [SerializeField]
    GameObject StoryContents;
    [SerializeField]
    TextMeshProUGUI storyTopText;
    [SerializeField]
    TextMeshProUGUI storyMiddleText;
    [SerializeField]
    TextMeshProUGUI storyBottomText;
    [SerializeField]
    TMP_FontAsset selectedFont;
    [SerializeField]
    TMP_FontAsset defaultFont;

    [SerializeField]
    GameObject storySelect;
    [SerializeField]
    GameObject statSelect;
    [SerializeField]
    GameObject skillSelect;


    [SerializeField]
    GameObject StatContents;
    
    [SerializeField]
    GameObject SkillContents;
    [SerializeField]
    Image Skill_1_Image;
    [SerializeField]
    TextMeshProUGUI Skill_1_Name;
    [SerializeField]
    TextMeshProUGUI Skill_1_Context;
    [SerializeField]
    Image Skill_2_Image;
    [SerializeField]
    TextMeshProUGUI Skill_2_Name;
    [SerializeField]
    TextMeshProUGUI Skill_2_Context;
    [SerializeField]
    Image Skill_Icon_1_Image;
    [SerializeField]
    Image Skill_Icon_2_Image;
    [SerializeField]
    Image backgound;
    [SerializeField]
    UI_SorcererGauge[] gaugeList;


    [SerializeField]
    public Sprite[] backgroundImage;
    [SerializeField]
    public VoicePackItem hojin;
    [SerializeField]
    public VoicePackItem siwoo;
    [SerializeField]
    public VoicePackItem sinwol;
    [SerializeField]
    public VoicePackItem seimei;
    [SerializeField]
    public VoicePackItem macia;
    [SerializeField]
    public VoicePackItem ulises;
    [SerializeField]
    public SoundRequesterSFX voiceShooter;

    UI_Sorcerer ui_SelectSorcerer;
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

        titleText.text = LocalizeManager.Instance.GetText("UI_SelectSorcerer");
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
                storyText.font = selectedFont;
                statText.font = defaultFont;
                skillText.font = defaultFont;

                StoryContents.gameObject.SetActive(true);
                StatContents.gameObject.SetActive(false);
                SkillContents.gameObject.SetActive(false);
                storySelect.SetActive(true);
                statSelect.SetActive(false);
                skillSelect.SetActive(false);
                
                break;
            case Buttons.StatBtn:
                storyText.font = defaultFont;
                statText.font = selectedFont;
                skillText.font = defaultFont;

                StoryContents.gameObject.SetActive(false);
                StatContents.gameObject.SetActive(true);
                SkillContents.gameObject.SetActive(false);
                storySelect.SetActive(false);
                statSelect.SetActive(true);
                skillSelect.SetActive(false);
                break;


            case Buttons.SkillBtn:
                storyText.font = defaultFont;
                statText.font = defaultFont;
                skillText.font = selectedFont;

                StoryContents.gameObject.SetActive(false);
                StatContents.gameObject.SetActive(false);
                SkillContents.gameObject.SetActive(true);
                storySelect.SetActive(false);
                statSelect.SetActive(false);
                skillSelect.SetActive(true);

                break;
            case Buttons.Select:
                DebugManager.Instance.PrintDebug("[SelectSorcerer] Selected Char to " + sorcererInfoID);
                UIStatus.Instance.selectedChar = sorcererInfoID;
                UIStatus.Instance.uI_SelectSorcerer.SetCharacterState();
                SetSorcerer();
                CloseUI<UI_SelectSorcererInfo>();
                break;
            default:
                break;
        }
    }

    public void SetSorcererInfo(int id, Dictionary<string, object> sorcerer, UI_Sorcerer parentUI)
    {
        sorcererInfoID = id;
        ui_SelectSorcerer = parentUI;
        string skill_1;
        string skill_2;

        foreach (KeyValuePair<string, object> val in sorcerer)
        {
            sorcererInfo.Add(val.Key, val.Value);

            if (val.Key == UIData.CharacterTableCol.CharacterName.ToString())
            {
                sorcererName.text = LocalizeManager.Instance.GetText(val.Value.ToString());
            }
            else if (val.Key == UIData.CharacterTableCol.MainShowImagePath.ToString())
            {
                Sprite imageSprite = Resources.Load<Sprite>($"{Define.UiArtsPath}/" + val.Value.ToString());
                if (imageSprite == null)
                    return;
                selectImage.sprite = imageSprite;

            }
            else if (val.Key == UIData.CharacterTableCol.IntroduceTextPath.ToString()) { 
                storyTopText.SetText(LocalizeManager.Instance.GetText(val.Value.ToString()));
            }

            else if(val.Key == UIData.CharacterTableCol.SkillID_01.ToString())
            {
                Skill_1_Name.text = GetSkillName(val.Value.ToString());
               // Skill_1_Image.sprite = GetSkillSprite(val.Value.ToString());
                Skill_1_Context.text = GetSkillDesc(val.Value.ToString());
                Skill_Icon_1_Image.sprite = GetSkillIconSprite(val.Value.ToString());
            }
            else if (val.Key == UIData.CharacterTableCol.SkillID_02.ToString())
            {
                Skill_2_Name.text = GetSkillName(val.Value.ToString());
                Skill_Icon_2_Image.sprite = GetSkillIconSprite(val.Value.ToString());
                Skill_2_Context.text = GetSkillDesc(val.Value.ToString());
            }
            else if (val.Key == UIData.CharacterTableCol.HP.ToString()) { 
                gaugeList[0].SetValue(Convert.ToInt32(val.Value),400);
            }
            else if (val.Key == UIData.CharacterTableCol.Attack.ToString())
            {
                gaugeList[1].SetValue(Convert.ToInt32(val.Value), 300);
            }
            else if (val.Key == UIData.CharacterTableCol.Shield.ToString())
            {
                gaugeList[2].SetValue(Convert.ToInt32(val.Value), 3);
            }
            else if (val.Key == UIData.CharacterTableCol.MoveSpeed.ToString())
            {
                gaugeList[3].SetValue(Convert.ToDouble(val.Value), 4.0);
            }
            else if (val.Key == UIData.CharacterTableCol.CriRatio.ToString())
            {
                gaugeList[4].SetValue(Convert.ToInt32(val.Value)/100, 100, "%");
            }
            else if (val.Key == UIData.CharacterTableCol.CriDamage.ToString())
            {
                gaugeList[5].SetValue(Convert.ToInt32(val.Value)/100, 400,"%");
            }
            else if (val.Key == UIData.CharacterTableCol.GetItemRange.ToString())
            {
                gaugeList[6].SetValue(Convert.ToDouble(val.Value), 5.0,"M");
            }
            else if (val.Key == UIData.CharacterTableCol.CoolDown.ToString())
            {
                gaugeList[7].SetValue(Convert.ToInt32(val.Value), 100);

            }
            SetVoiceResource(sorcererInfoID);

        }
    }

    string GetSkillName(string skillID)
    {
        return LocalizeManager.Instance.GetText(UIData.SkillData[skillID]["Name"].ToString());
    }
    string GetSkillDesc(string skillID)
    {
        return LocalizeManager.Instance.GetText(UIData.SkillData[skillID]["Desc"].ToString());
    }
    Sprite GetSkillIconSprite(string skillID)
    {
        return ResourcesManager.Load<Sprite>(UIData.SkillData[skillID]["Icon"].ToString());
    }

    public void SetVoiceResource(int sorcererID)
    {
        switch (sorcererID)
        {
            case (int)UIStatus.Sorcerers.hojin:
                voiceShooter.soundPackItems[0].audioClipList=hojin.GetSoundList();
                backgound.sprite =backgroundImage[0];
                break;
            case (int)UIStatus.Sorcerers.sinwol:
                voiceShooter.soundPackItems[0].audioClipList = sinwol.GetSoundList();
                backgound.sprite = backgroundImage[0];
                break;
            case (int)UIStatus.Sorcerers.siWoo:
                voiceShooter.soundPackItems[0].audioClipList = siwoo.GetSoundList();
                backgound.sprite = backgroundImage[2];
                break;
            case (int)UIStatus.Sorcerers.seimei:
                voiceShooter.soundPackItems[0].audioClipList = seimei.GetSoundList();
                backgound.sprite = backgroundImage[1];
                break;
            case (int)UIStatus.Sorcerers.ulises:
                voiceShooter.soundPackItems[0].audioClipList = ulises.GetSoundList();
                backgound.sprite = backgroundImage[1];
                break;
            case (int)UIStatus.Sorcerers.macia:
                voiceShooter.soundPackItems[0].audioClipList = macia.GetSoundList();
                backgound.sprite = backgroundImage[2];
                break;
            default:
                break;
        }
        voiceShooter.ChangeSituation(SoundSituation.SOUNDSITUATION.INTERECT);
    }

    async void SetSorcerer()
    {
        if (!SteamManager.Initialized) { return; }
        await APIManager.Instance.SetSorcererRequest(UIStatus.Instance.GetSorcerer(sorcererInfoID));
    }
}
