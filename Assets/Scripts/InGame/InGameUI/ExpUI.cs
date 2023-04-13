using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    private Image expImage;
    private Text expText;

    private void Start()
    {
        expImage = GetComponent<Image>();

        expText = GetComponentInChildren<Text>();
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
