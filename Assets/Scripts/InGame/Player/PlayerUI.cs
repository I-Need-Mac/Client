using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Player player;

    private void Awake()
    {
        slider.maxValue = player.playerData.hp;
        slider.value = player.playerData.hp;
    }

    private void FixedUpdate()
    {
        
    }
}
