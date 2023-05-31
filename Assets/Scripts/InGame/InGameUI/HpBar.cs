using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Image hpBar;

    private void Start()
    {
        hpBar = GetComponent<Image>();
    }

    private void Update()
    {
        transform.position = CameraManager.Instance.cam.WorldToScreenPoint(GameManager.Instance.player.transform.position + new Vector3(0, -0.6f, 0));
        hpBar.fillAmount = (float)GameManager.Instance.player.playerManager.playerData.currentHp / GameManager.Instance.player.playerManager.playerData.hp;
    }
}
