using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    private Text expText;

    private void Start()
    {
        expText = GetComponentInChildren<Text>();
        expText.color = Color.white;
        expText.fontSize = 25;

    }

    private void Update()
    {
        expText.text = $"{GameManager.Instance.player.exp}/{GameManager.Instance.player.needExp}";
    }
}
