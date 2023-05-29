using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GameOverUI gameOverUi;
    private PlayerStatusUI statusUi;
    private LevelUpUI levelUi;

    private void Awake()
    {
        gameOverUi = GetComponentInChildren<GameOverUI>();
        statusUi = GetComponentInChildren<PlayerStatusUI>();
        levelUi = GetComponentInChildren<LevelUpUI>();
    }

    private void Start()
    {
        gameOverUi.gameObject.SetActive(false);
        levelUi.gameObject.SetActive(false);
    }

    public void LevelTextChange(int level)
    {
        statusUi.levelText.text = $"Lv.{level}";
    }

    public void SkillSelectWindowOpen()
    {
        levelUi.gameObject.SetActive(true);
        Time.timeScale = 0f;
        levelUi.skills.Clear();
        levelUi.SkillBoxInit(3);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUi.gameObject.SetActive(true);
    }
    
}
