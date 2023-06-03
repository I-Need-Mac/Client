using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected Collider2D projectileCollider;

    public float totalDamage { get; private set; }
    public bool isHit { get; private set; }
    public SkillData skillData { get; private set; }

    private void Awake()
    {
        if (!TryGetComponent(out projectileCollider))
        {
            projectileCollider = GetComponentInChildren<Collider2D>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        projectileCollider.enabled = true;
        isHit = false;
    }

    public void SetAnimation(Sprite sprite, RuntimeAnimatorController controller)
    {
        spriteRenderer.sprite = sprite;
        animator.runtimeAnimatorController = controller;
    }

    public virtual void SetProjectile(SkillData skillData)
    {
        this.skillData = skillData;
        projectileCollider.isTrigger = this.skillData.isPenetrate;
        transform.localScale *= this.skillData.projectileSizeMulti;

        totalDamage = GameManager.Instance.player.playerManager.TotalDamage(skillData.damage);
    }

    public void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void CollisionRadius(float radius)
    {
        ((CircleCollider2D)projectileCollider).radius = radius;
    }

    public void CollisionPower(bool flag)
    {
        projectileCollider.enabled = flag;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayerConstant.MONSTER)
        {
            isHit = true;
            DebugManager.Instance.PrintDebug("[TEST]: Hit");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayerConstant.MONSTER)
        {
            isHit = false;
            DebugManager.Instance.PrintDebug("[TEST]: Out");
        }
    }

    private void OnBecameInvisible()
    {
        Invoke("Remove", skillData.duration * 2);
    }

    protected void Remove()
    {
        SkillManager.Instance.DeSpawnProjectile(this);
    }

}
