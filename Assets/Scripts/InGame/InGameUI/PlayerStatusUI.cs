using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public TextMeshProUGUI levelText { get; private set; }

    private void Start()
    {
        levelText = transform.Find("NameBox").GetComponentInChildren<TextMeshProUGUI>();
        levelText.text = $"Lv.{1}";
    }

}
