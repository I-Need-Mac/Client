using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : SingletonBehaviour<Timer>
{
    private Text timeText;

    public int currentTime { get; private set; } = 0;

    private void Start()
    {
        timeText = GetComponent<Text>();
        timeText.color = Color.white;
        timeText.fontSize = 50;
    }

    private void Update()
    {
        currentTime = (int)(Time.time * 1000);
        timeText.text = $"{currentTime/1000/60:00}:{currentTime/1000%60:00}";
    }
}
