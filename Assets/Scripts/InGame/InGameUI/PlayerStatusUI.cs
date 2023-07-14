using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    public TextMeshProUGUI levelText { get; private set; }
    public Image iconImage { get; private set; }
    private Transform nameBox;

    private void Start()
    {
        nameBox = transform.Find("NameBox");
        levelText = nameBox.GetComponentInChildren<TextMeshProUGUI>();
        iconImage = nameBox.Find("CharacterIcon").GetComponent<Image>();
    }
}
