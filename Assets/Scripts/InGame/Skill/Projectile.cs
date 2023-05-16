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

    public SkillData skillData { get; private set; }

    private void Awake()
    {
        if (TryGetComponent(out projectileCollider))
        {
            projectileCollider = GetComponentInChildren<Collider2D>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        projectileCollider.enabled = true;
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
    }

    private void OnBecameInvisible()
    {
        Invoke("Remove", skillData.duration);
    }

    protected void Remove()
    {
        SkillManager.Instance.DeSpawnProjectile(this);
    }

    //private bool who; //true: player, false: monster
    //private Player player;
    //private Monster monster;
    //private Transform shooter;
    //private WaitForSeconds coolTime;

    //public SkillData skillData { get; private set; }

    //private Skill(string skillId)
    //{
    //    skillData = new SkillData();
    //    SkillDataLoad(FindSkill(skillId));
    //    coolTime = new WaitForSeconds(skillData.coolTime);
    //}

    //public Skill(string skillId, Player player) : this(skillId)
    //{
    //    who = true;
    //    this.player = player;
    //    shooter = this.player.transform;
    //}

    //public Skill(string skillId, Monster monster) : this(skillId)
    //{
    //    who = false;
    //    this.monster = monster;
    //    shooter = this.monster.transform;
    //}

    //private Vector2 LookDirection()
    //{
    //    return who ? player.lookDirection : monster.lookDirection;
    //}

    ////Satellite type skill activation
    //public IEnumerator SatelliteSkill()
    //{
    //    for (int i = 0; i < skillData.projectileCount; i++)
    //    {
    //        Projectile projectile = ProjectilePoolManager.Instance.SpawnProjectile(skillData);
    //        projectile.angle = (360f / skillData.projectileCount) * (i + 1);
    //        Vector3 spawnPos = new Vector3(Mathf.Cos(projectile.angle * Mathf.Deg2Rad), Mathf.Sin(projectile.angle * Mathf.Deg2Rad), 0);
    //        projectile.Fire(shooter, spawnPos);
    //    }
    //    yield return coolTime; //지속시간인데 이거 물어봐야함 스킬데이터에없음
    //}

    ////Protect type skill activation
    //public IEnumerator ProtectSkill()
    //{
    //    Projectile projectile = ProjectilePoolManager.Instance.SpawnProjectile(skillData);
    //    projectile.Fire(shooter, Vector3.zero);
    //    yield return null;
    //}

    ////Shoot type skill activation
    //public IEnumerator ShootSkill()
    //{
    //    if (!skillData.isEffect)
    //    {
    //        yield return coolTime;
    //    }
    //    while (true)
    //    {
    //        Vector2 direction = LookDirection();
    //        for (int i = 0; i < skillData.projectileCount; i++)
    //        {
    //            Projectile projectile = ProjectilePoolManager.Instance.SpawnProjectile(skillData);
    //            projectile.Fire(shooter, direction);
    //            yield return new WaitForSeconds(0.2f); //발사 간격
    //        }
    //        yield return coolTime;
    //    }
    //}

    //public void SkillLevelUp()
    //{
    //    SkillDataLoad(FindSkill((skillData.skillId + 1).ToString()));
    //}

    //#region Skill Load

    //private void SkillDataLoad(Dictionary<string, object> skillInfo)
    //{
    //    if (skillInfo == null)
    //    {
    //        DebugManager.Instance.PrintDebug("존재하지 않는 스킬입니다");
    //        return;
    //    }
    //    skillData.SetCoolTime(Convert.ToInt32(skillInfo["Cooltime"]));
    //    skillData.SetAttackDistance(Convert.ToInt32(skillInfo["AttackDistance"]));
    //    skillData.SetDamage(Convert.ToInt32(skillInfo["Damage"]));
    //    skillData.SetSkillEffectParam(Convert.ToInt32(skillInfo["SkillEffectParam"]));
    //    skillData.SetSkillCut(Convert.ToBoolean(Convert.ToString(skillInfo["Skill_Cut"]).ToLower()));
    //    skillData.SetIsEffect(Convert.ToBoolean(Convert.ToString(skillInfo["IsEffect"]).ToLower()));
    //    skillData.SetIsUltimate(Convert.ToBoolean(Convert.ToString(skillInfo["IsUltimate"]).ToLower()));
    //    skillData.SetName(Convert.ToString(skillInfo["Name"]));
    //    skillData.SetDesc(Convert.ToString(skillInfo["Desc"]));
    //    skillData.SetIcon(Convert.ToString(skillInfo["Icon"]));
    //    skillData.SetCutDire(Convert.ToString(skillInfo["Cut_dire"]));
    //    skillData.SetSkillImage(Convert.ToString(skillInfo["SkillImage"]));
    //    skillData.SetSkillEffect((SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), Convert.ToString(skillInfo["SkillEffect"]).ToUpper()));
    //    skillData.SetSkillTarget((SKILL_TARGET)Enum.Parse(typeof(SKILL_TARGET), Convert.ToString(skillInfo["SkillTarget"]).ToUpper()));
    //    //skillData.SetCalcDamageType((CALC_DAMAGE_TYPE)Enum.Parse(typeof(CALC_DAMAGE_TYPE), Convert.ToString(skillInfo["SkillCalcType"]).ToUpper()));

    //    skillData.SetProjectileCount(Convert.ToInt32(skillInfo["ProjectileCount"]));
    //    skillData.SetSpeed(Convert.ToInt32(skillInfo["Speed"]));
    //    skillData.SetSplashRange(Convert.ToInt32(skillInfo["SplashRange"]));
    //    skillData.SetProjectileSizeMulti(Convert.ToInt32(skillInfo["ProjectileSizeMulti"]));
    //    skillData.SetIsPenetrate(Convert.ToBoolean(skillInfo["IsPenetrate"]));
    //    skillData.SetProjectileType((PROJECTILE_TYPE)Enum.Parse(typeof(PROJECTILE_TYPE), Convert.ToString(skillInfo["ProjectileType"]).ToUpper()));
    //    skillData.SetSkillPrefabPath(skillInfo["SkillPrefabPath"].ToString());
    //}

    //private Dictionary<string, object> FindSkill(string skillId)
    //{
    //    Dictionary<string, Dictionary<string, object>> skillTable = CSVReader.Read("SkillTable");

    //    if (skillTable.ContainsKey(skillId))
    //    {
    //        skillData.SetSkillId(Convert.ToInt32(skillId));
    //        return skillTable[skillId];
    //    }
    //    //찾는 스킬이 없을 경우 null
    //    return null;
    //}
    //#endregion
}
