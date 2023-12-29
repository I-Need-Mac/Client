using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameMain : UI_Popup
{
    enum GameObjects
    {
        StoryMode,
        SelectSorcerer,
        Soul,
        Sorcere,
    }

    [SerializeField]
    TextMeshProUGUI storyModeText;
    [SerializeField]
    TextMeshProUGUI selectSorcererText;
    [SerializeField]
    TextMeshProUGUI soulText;
    [SerializeField]
    TextMeshProUGUI sorcereText;

    void Start()
    {
        GetStartData();
        Bind<GameObject>(typeof(GameObjects));

        Array objectValue = Enum.GetValues(typeof(GameObjects));
        for (int i = 0; i < objectValue.Length; i++)
        {
            BindUIEvent(GetGameObject(i).gameObject, (PointerEventData data) => { OnClickObject(data); }, Define.UIEvent.Click);
        }

        storyModeText.text = LocalizeManager.Instance.GetText("UI_StoryMode");
        selectSorcererText.text = LocalizeManager.Instance.GetText("UI_SelectSorcerer");
        soulText.text = LocalizeManager.Instance.GetText("UI_Soul");
        sorcereText.text = LocalizeManager.Instance.GetText("UI_Sorcer");
        UIManager.Instance.CloseUI<UI_StartMain>();

        SoundManager.Instance.RefindBGMRequester();
    }
    private void Update()
    {
        OnPressKeyDown();
    }

    public void OnPressKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!UIManager.Instance.IsUiPopup("UI_ESCPopup"))
                UIManager.Instance.OpenUI<UI_ESCPopup>();
            else
                UIManager.Instance.CloseUI<UI_ESCPopup>();
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
            case GameObjects.StoryMode:
                UIManager.Instance.OpenUI<UI_StoryMain>();
                break;
            case GameObjects.SelectSorcerer:
                UIManager.Instance.OpenUI<UI_SelectSorcerer>();
                break;
            case GameObjects.Soul:
                UIManager.Instance.OpenUI<UI_Hon>();
                break;
            case GameObjects.Sorcere:
                UIManager.Instance.OpenUI<UI_Jusulso>();
                break;
            default:
                break;
        }
    }
    async void GetStartData()
    {
        if (!SteamManager.Initialized) { return; }
        string name = SteamUser.GetSteamID().ToString();
        await APIManager.Instance.StartGame(name, UIStatus.Instance.nickname);
    }
}
