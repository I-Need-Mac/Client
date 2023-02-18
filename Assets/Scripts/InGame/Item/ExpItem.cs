using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    [SerializeField] private Sprite[] types; //초록색 == 0, 빨간색 == 1, 파란색 == 2

    private float exp;

    private void Awake()
    {
        gameObject.tag = "Exp";
        render = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
        itemCollider.isTrigger = true;
    }

    //몬스터로부터 정보를 받아서 경험치 아이템 셋팅(경험치량에 따른 혼 이미지 종류 등)
    public void ReceiveInfo(int type, float exp)
    {
        render.sprite = types[type];
        this.exp = exp;
    }

}
