using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const string BOUNCE_PATH = "Prefabs/InGame/Skill/Bounce";

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
        if (projectileCollider != null)
        {
            projectileCollider.enabled = true;
        }
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
        if (projectileCollider != null)
        {
            projectileCollider.isTrigger = true;
        }
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
        if (collision.TryGetComponent(out Monster monster))
        {
            isHit = true;
            SkillEffect(monster);

            if (!skillData.isPenetrate)
            {
                Remove();
            }
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

    //private void OnBecameInvisible()
    //{
    //    Invoke("Remove", skillData.duration * 2);
    //}

    protected void Remove()
    {
        SkillManager.Instance.DeSpawnProjectile(this);
    }

    protected void SkillEffect(Monster target)
    {
        int count = skillData.skillEffect.Count;
        for (int i = 0; i < count; i++)
        {
            float param = float.Parse(skillData.skillEffectParam[i]);
            switch (skillData.skillEffect[i])
            {
                case SKILL_EFFECT.EXPLORE:
                    Explore(param);
                    break;
                case SKILL_EFFECT.MOVEUP:
                    StartCoroutine(MoveUp(param));
                    break;
                case SKILL_EFFECT.DRAIN:
                    Drain(param);
                    break;
                case SKILL_EFFECT.STUN:
                case SKILL_EFFECT.SLOW:
                case SKILL_EFFECT.KNOCKBACK:
                case SKILL_EFFECT.EXECUTE:
                case SKILL_EFFECT.RESTRAINT:
                case SKILL_EFFECT.PULL:
                    target.SkillEffectActivation(skillData.skillEffect[i], param);
                    break;
                default:
                    DebugManager.Instance.PrintDebug("[ERROR]: 없는 스킬 효과입니다");
                    break;
            }
        }
    }

    private void Explore(float n)
    {
        if (UnityEngine.Random.Range(0, 100) < n)
        {
            List<Transform> targets = Scanner.RangeTarget(transform, skillData.splashRange, (int)LayerConstant.MONSTER);
            foreach (Transform target in targets)
            {
                if (target.TryGetComponent(out Monster monster))
                {
                    monster.monsterData.SetCurrentHp(monster.monsterData.currentHp - (int)totalDamage);
                }
            }
        }
    }

    private IEnumerator MoveUp(float n)
    {
        int originSpeed = GameManager.Instance.player.playerManager.playerData.moveSpeed;
        GameManager.Instance.player.playerManager.playerData.SetMoveSpeed((int)(originSpeed * n * 0.01f));
        yield return new WaitForSeconds(skillData.duration);
        GameManager.Instance.player.playerManager.playerData.SetMoveSpeed(originSpeed);
    }

    private void Drain(float n)
    {
        float hp = GameManager.Instance.player.playerManager.playerData.currentHp + (totalDamage * n * 0.01f);
        DebugManager.Instance.PrintDebug("HPTest2: " + hp);
        GameManager.Instance.player.playerManager.playerData.SetCurrentHp((int)hp);
    }



}
