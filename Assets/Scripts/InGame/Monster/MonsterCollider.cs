using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    private CapsuleCollider2D attackCollider;
    private float sizeX;

    private void Awake()
    {
        attackCollider = GetComponent<CapsuleCollider2D>();
        attackCollider.isTrigger = true;
        sizeX = attackCollider.size.x;
        gameObject.layer = (int)LayerConstant.HIT;
    }

    public void SetAttackDistance(float atkDistance)
    {
        Vector2 size = attackCollider.size;
        size.x = sizeX + atkDistance;
        attackCollider.size = size;

        Vector2 offSet = attackCollider.offset;
        offSet.x = -atkDistance * 0.5f;
        attackCollider.offset = offSet;

        attackCollider.enabled = false;
    }

    public void AttackColliderSwitch(bool flag)
    {
        attackCollider.enabled = flag;
    }

}
