using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public string monsterName { get; private set; }
    public int hp { get; private set; }
    public int attack { get; private set; }
    public int moveSpeed { get; private set; }
    public int atkDistance { get; private set; }
    public int skillID { get; private set; }
    public int expDropRate { get; private set; }
    public int treasureDropRate { get; private set; }
    public int moneyDropRate { get; private set; }
    public string monsterImage { get; private set; }

    public void SetMonsterName(string monsterName) { this.monsterName = monsterName; }
    public void SetHp(int hp) { this.hp = hp; }
    public void SetAttack(int attack) { this.attack = attack; }
    public void SetMoveSpeed(int moveSpeed) { this.moveSpeed = moveSpeed; }
    public void SetAtkDistance(int atkDistance) { this.atkDistance = atkDistance; }
    public void SetSkillID(int skillID) { this.skillID = skillID; }
    public void SetExpDropRate(int expDropRate) { this.expDropRate = expDropRate; }
    public void SetTreasureDropRate(int treasureDropRate) { this.treasureDropRate = treasureDropRate; }
    public void SetMoneyDropRate(int moneyDropRate) { this.moneyDropRate = moneyDropRate; }
    public void SetMonsterImage(string monsterImage) { this.monsterImage = monsterImage; }
}