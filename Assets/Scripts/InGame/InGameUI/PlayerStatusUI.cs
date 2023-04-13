using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public Text levelText { get; private set; }

    private void Start()
    {
        levelText = transform.Find("NameBox").GetComponentInChildren<Text>();
        levelText.text = $"Lv.{1}";
    }

}
