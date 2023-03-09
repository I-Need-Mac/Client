using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timeText;
    private float time;

    private void Start()
    {
        timeText = GetComponent<Text>();
        timeText.color = Color.white;
    }

    private void Update()
    {
        timeText.text = ((int)Time.time).ToString();
    }
}
