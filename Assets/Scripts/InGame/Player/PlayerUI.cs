using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Button[] btns;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        foreach (Button btn in btns)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.AddListener(SkillSelectWindowClose);
        }
    }

    public void SkillSelectWindowOpen()
    {
        Time.timeScale = 0f;
        foreach (Button btn in btns)
        {
            btn.gameObject.SetActive(true);
        }
    }

    public void SkillSelectWindowClose()
    {
        Time.timeScale = 1.0f;
        //Text selectedBtn = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
        //PlayerManager.Instance.playerData.SetSkill(new Skill(selectedBtn.text, GameManager.Instance.player));
        foreach (Button btn in btns)
        {
            btn.gameObject.SetActive(false);
        }
    }
}
