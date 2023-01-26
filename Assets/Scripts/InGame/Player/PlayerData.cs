
using System.Collections.Generic;
using System.Linq;

public class PlayerData
{
    public string characterName { get; private set; }               //캐릭터 이름
    public int hp { get; private set; }                             //체력
    public int attack { get; private set; }                         //공격력
    public int criRatio { get; private set; }                       //크리티컬 확률
    public int criDamage { get; private set; }                      //크리티컬 데미지
    public int coolDown { get; private set; }                       //쿨타임 감소량
    public int hpRegen { get; private set; }                        //체젠량
    public int shield { get; private set; }                         //쉴드 개수
    public int projectileAdd { get; private set; }                  //투사체 증가 개수
    public int moveSpeed { get; private set; }                      //이동 속도
    public int getItemRange { get; private set; }                   //아이템 획득 범위
    ////스킬들을 저장할 Dic (스킬이름, 스킬)
    //public Dictionary<string, Skill> skills { get; private set; } = new Dictionary<string, Skill>();
    public List<Skill> skills { get; private set; } = new List<Skill>();

    //Property setter를 사용할 경우 get이 같이 참조되어 setter를 따로 생성
    public void SetCharacterName(string characterName) { this.characterName = characterName; }
    public void SetHp(int hp) { this.hp = hp; }
    public void SetAttack(int attack) { this.attack = attack; }
    public void SetCriRatio(int criRatio) { this.criRatio = criRatio; }
    public void SetCriDamage(int criDamage) { this.criDamage = criDamage; }
    public void SetCoolDown(int coolDown) { this.coolDown = coolDown; }
    public void SetHpRegen(int hpRegen) { this.hpRegen = hpRegen; }
    public void SetShield(int shield) { this.shield = shield; }
    public void SetProjectileAdd(int projectileAdd) { this.projectileAdd = projectileAdd; }
    public void SetMoveSpeed(int moveSpeed) { this.moveSpeed = moveSpeed; }
    public void SetGetItemRange(int getItemRange) { this.getItemRange = getItemRange; }
    //public void SetSkill(Skill skill) { skills.Add(skill.skillData.name, skill); }
    public void SetSkill(Skill skill) { skills.Add(skill); }

    //public void SetStat(PlayerData playerData)
    //{
    //    characterName = playerData.characterName;
    //    hp = playerData.hp;
    //    attack = playerData.attack;
    //    criRatio = playerData.criRatio;
    //    criDamage = playerData.criDamage;
    //    coolDown = playerData.coolDown;
    //    hpRegen = playerData.hpRegen;
    //    shield = playerData.shield;
    //    projectileAdd = playerData.projectileAdd;
    //    moveSpeed = playerData.moveSpeed;
    //    getItemRange = playerData.getItemRange;
    //    skills = playerData.skills.ToList();
    //}
}