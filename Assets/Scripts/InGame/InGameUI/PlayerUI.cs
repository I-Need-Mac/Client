using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //private Canvas canvas;
    private GameOverUI gameOverUi;
    private PlayerStatusUI statusUi;
    private PlayerSkillUI skillUi;

    private void Awake()
    {
        //canvas = GetComponent<Canvas>();
        //canvas.worldCamera = Camera.main;
        gameOverUi = GetComponentInChildren<GameOverUI>();
        statusUi = GetComponentInChildren<PlayerStatusUI>();
        skillUi = GetComponentInChildren<PlayerSkillUI>();

        gameOverUi.gameObject.SetActive(false);
        skillUi.gameObject.SetActive(false);
    }

    public void LevelTextChange(int level)
    {
        statusUi.levelText.text = $"Lv.{level}";
    }

    public void test()
    {
        skillUi.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUi.gameObject.SetActive(true);
    }
    
}
