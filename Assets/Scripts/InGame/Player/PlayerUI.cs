using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> skillTable;

    private Button[] selectBtns;
    private Text[] skillInfoText;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        skillTable = CSVReader.Read("SkillTable");

        selectBtns = GetComponentsInChildren<Button>();
        skillInfoText = GetComponentsInChildren<Text>().Where(text => !text.text.Equals("Refresh")).ToArray();

        foreach (Button btn in selectBtns)
        {
            if (btn.name.Contains("SkillButton"))
            {
                btn.onClick.AddListener(SkillSelectWindowClose);
            }
            else
            {
                btn.onClick.AddListener(RefreshSkillWindow);
            }
            
            btn.gameObject.SetActive(false);
        }
        SkillInfoAssign();
    }

    private void SkillInfoAssign()
    {
        foreach (Text info in skillInfoText)
        {
            info.text = skillTable.Keys.ElementAt(UnityEngine.Random.Range(0, skillTable.Count - 1));
        }
    }

    public void SkillSelectWindowOpen()
    {
        Time.timeScale = 0f;
        foreach (Button btn in selectBtns)
        {
            btn.gameObject.SetActive(true);
        }
    }

    private void SkillSelectWindowClose()
    {
        Text selectedBtn = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
        DebugManager.Instance.PrintDebug("##: " + selectedBtn.name);
        GameManager.Instance.player.Fire(Convert.ToInt32(selectedBtn.text));
        foreach (Button btn in selectBtns)
        {
            btn.gameObject.SetActive(false);
        }
        Time.timeScale = 1.0f;
    }

    private void RefreshSkillWindow()
    {
        SkillInfoAssign();
    }
}
