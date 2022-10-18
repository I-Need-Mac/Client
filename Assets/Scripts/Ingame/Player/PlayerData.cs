using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string characterName { get; private set; }
    public int hp { get; private set; }
    public int atk { get; private set; }
    public int criRatio { get; private set; }
    public int criDamage { get; private set; }
    public int coolDown { get; private set; }
    public int hpRegen { get; private set; }
    public int shield { get; private set; }
    public int projectileAdd { get; private set; }
    public int moveSpeed { get; private set; }
    public int getItemRange { get; private set; }
    public int ultimateID { get; private set; }
    public int skillID { get; private set; }

    public SkillData ultimateData;
    public SkillData skillData;

    public void SetCharacterName(string characterName) { this.characterName = characterName; }
    public void SetHp(int hp) { this.hp = hp; }
    public void SetAtk(int atk) { this.atk = atk; }
    public void SetCriRatio(int criRatio) { this.criRatio = criRatio; }
    public void SetCriDamage(int criDamage) { this.criDamage = criDamage; }
    public void SetCoolDown(int coolDown) { this.coolDown = coolDown; }
    public void SetHpRegen(int hpRegen) { this.hpRegen = hpRegen; }
    public void SetShield(int shield) { this.shield = shield; }
    public void SetProjectileAdd(int projectileAdd) { this.projectileAdd = projectileAdd; }
    public void SetMoveSpeed(int moveSpeed) { this.moveSpeed = moveSpeed; }
    public void SetGetItemRange(int getItemRange) { this.getItemRange = getItemRange; }
    public void SetUltimateID(int ultimateID) { this.ultimateID = ultimateID; }
    public void SetskillID(int skillID) { this.skillID = skillID; }
    public void SetUltimateData(SkillData ultimateData) { this.ultimateData = ultimateData; }
    public void SetSkillData(SkillData skillData) { this.skillData = skillData; }
}