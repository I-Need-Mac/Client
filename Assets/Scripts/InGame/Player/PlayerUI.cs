using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Player player;
    [SerializeField] private Text playerLevel;
    [SerializeField] private Text playerName;

    private void Start()
    {
        DebugManager.Instance.PrintDebug(player.playerData.hp);
        slider.maxValue = player.playerData.hp;
        slider.value = slider.maxValue;
        playerLevel.text += "1";
        playerName.text = player.playerData.characterName;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.K))
        {
            player.ModifyHp(0, 5);
            DebugManager.Instance.PrintDebug(slider.value);
        }
        slider.value = player.hp;
    }
}
