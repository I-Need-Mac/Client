
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [field: SerializeField] public string characterName { private set; get;}            //캐릭터 이름
    [field: SerializeField] public int hp { private set; get; }                                 //체력
    [field: SerializeField] public int currentHp { private set; get; }
    [field: SerializeField] public float attack { private set; get; }                         //공격력
    [field: SerializeField] public int criRatio { private set; get; }                       //크리티컬 확률
    [field: SerializeField] public float criDamage { private set; get; }                      //크리티컬 데미지
    [field: SerializeField] public int coolDown { private set; get; }                       //쿨타임 감소량
    [field: SerializeField] public int hpRegen { private set; get; }                        //체젠량
    [field: SerializeField] public int shield { private set; get; }                         //쉴드 개수
    [field: SerializeField] public int projectileAdd { private set; get; }                 //투사체 증가 개수
    [field: SerializeField] public int moveSpeed { private set; get; }                      //이동 속도
    [field: SerializeField] public int getItemRange { private set; get; }                  //아이템 획득 범위
    [field: SerializeField] public string characterPrefabPath { private set; get; }
    //public Dictionary<int, SkillInfo> skills { get; private set; }

    //Property setter를 사용할 경우 get이 같이 참조되어 setter를 따로 생성
    public void SetCharacterName(string characterName) { this.characterName = characterName; }
    public void SetHp(int hp) { this.hp = hp; }
    public void SetCurrentHp(int currentHp) { this.currentHp = currentHp; }
    public void SetAttack(int attack) { this.attack = attack * 0.01f; }
    public void SetCriRatio(int criRatio) { this.criRatio = criRatio; }
    public void SetCriDamage(float criDamage) { this.criDamage = criDamage; }
    public void SetCoolDown(int coolDown) { this.coolDown = coolDown; }
    public void SetHpRegen(int hpRegen) { this.hpRegen = hpRegen; }
    public void SetShield(int shield) { this.shield = shield; }
    public void SetProjectileAdd(int projectileAdd) { this.projectileAdd = projectileAdd; }
    public void SetMoveSpeed(int moveSpeed) { this.moveSpeed = moveSpeed; }
    public void SetGetItemRange(int getItemRange) { this.getItemRange = getItemRange; }
    
    public void HpRegen()
    {
        this.currentHp += this.hpRegen;
        if (this.currentHp > this.hp)
        {
            this.currentHp = this.hp;
        }
    }
}

//public struct SkillInfo
//{
//    public Skill skill { get; private set; }
//    public IEnumerator type { get; private set; }

//    public SkillInfo(Skill skill, IEnumerator type)
//    {
//        this.skill = skill;
//        this.type = type;
//    }
//}