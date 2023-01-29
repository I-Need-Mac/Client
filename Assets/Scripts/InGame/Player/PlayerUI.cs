using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider expBar;
    [SerializeField] private Player player;
    [SerializeField] private Text playerLevel;
    [SerializeField] private Text playerName;

    private void Start()
    {
        //DebugManager.Instance.PrintDebug(player.playerData.hp);
        //hpBar.maxValue = player.playerData.hp;
        //hpBar.value = hpBar.maxValue;
        //playerLevel.text += "1";
        //playerName.text = player.playerData.characterName;
    }

    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.K))
        //{
        //    player.ModifyHp(0, 5);
        //    DebugManager.Instance.PrintDebug(hpBar.value);
        //}
        //hpBar.value = player.hp;
    }
}
