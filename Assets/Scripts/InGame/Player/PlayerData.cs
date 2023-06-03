
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerData
{
    public string characterName { get; private set; }               //캐릭터 이름
    public int hp { get; private set; }                             //체력
    public int currentHp { get; private set; }
    public float attack { get; private set; }                         //공격력
    public int criRatio { get; private set; }                       //크리티컬 확률
    public float criDamage { get; private set; }                      //크리티컬 데미지
    public int coolDown { get; private set; }                       //쿨타임 감소량
    public int hpRegen { get; private set; }                        //체젠량
    public int shield { get; private set; }                         //쉴드 개수
    public int projectileAdd { get; private set; }                  //투사체 증가 개수
    public int moveSpeed { get; private set; }                      //이동 속도
    public int getItemRange { get; private set; }                   //아이템 획득 범위
    public string characterPrefabPath { get; private set; }
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