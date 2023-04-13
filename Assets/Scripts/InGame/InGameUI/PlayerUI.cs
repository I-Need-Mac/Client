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

    private PlayerStatusUI statusUi;
    private PlayerSkillUI skillUi;

    private void Awake()
    {
        //canvas = GetComponent<Canvas>();
        //canvas.worldCamera = Camera.main;

        statusUi = GetComponentInChildren<PlayerStatusUI>();
        skillUi = GetComponentInChildren<PlayerSkillUI>();
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
    
}
