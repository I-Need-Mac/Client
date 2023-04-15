using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    private Image expImage;
    private TextMeshProUGUI expText;

    private void Start()
    {
        expImage = GetComponent<Image>();

        expText = GetComponentInChildren<TextMeshProUGUI>();
        expText.color = Color.white;
        expText.fontSize = 25;
    }

    private void Update()
    {
        int exp = GameManager.Instance.player.exp;
        int needExp = GameManager.Instance.player.needExp;

        expImage.fillAmount = exp / (float)needExp;
        expText.text = $"{exp}/{needExp}";
    }
}
