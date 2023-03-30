using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public string monsterName { get; private set; }
    public float sizeMultiple { get; private set; }
    public int hp { get; private set; }
    public int attack { get; private set; }
    public int moveSpeed { get; private set; }
    public float atkSpeed { get; private set; }
    public int viewDistance { get; private set; }
    public int atkDistance { get; private set; }
    public int skillID { get; private set; }
    public string groupSource { get; private set; }
    public int groupSourceRate { get; private set; }
    public string monsterPrefabPath { get; private set; }
    public AttackTypeConstant attackType { get; private set; }

    public void SetMonsterName(string monsterName) { this.monsterName = monsterName; }
    public void SetSizeMultiple(float sizeMultiple) { this.sizeMultiple = sizeMultiple; }
    public void SetHp(int hp) { this.hp = hp; }
    public void SetAttack(int attack) { this.attack = attack; }
    public void SetMoveSpeed(int moveSpeed) { this.moveSpeed = moveSpeed; }
    public void SetAtkSpeed(float atkSpeed) { this.atkSpeed = atkSpeed; }
    public void SetViewDistance(int viewDistance) { this.viewDistance = viewDistance; }
    public void SetAtkDistance(int atkDistance) { this.atkDistance = atkDistance; }
    public void SetSkillID(int skillID) { this.skillID = skillID; }
    public void SetGroupSource(string groupSource) { this.groupSource = groupSource; }
    public void SetGroupSourceRate(int groupSourceRate) { this.groupSourceRate = groupSourceRate; }
    public void SetMonsterPrefabPath(string monsterPrefabPath) { this.monsterPrefabPath = monsterPrefabPath; }
    public void SetAttackType(AttackTypeConstant attackType) { this.attackType = attackType; }
}