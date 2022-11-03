using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string CharacterName { get; private set; }  //캐릭터 이름
    public int HP { get; private set; }                 //체력
    public int Attack { get; private set; }             //공격력
    public int CriRatio { get; private set; }          //크리티컬 확률
    public int CriDamage { get; private set; }         //크리티컬 데미지
    public int CoolDown { get; private set; }          //쿨타임 감소량
    public int HPRegen { get; private set; }           //체젠량
    public int Shield { get; private set; }             //쉴드 개수
    public int ProjectileAdd { get; private set; }     //투사체 증가 개수
    public int MoveSpeed { get; private set; }         //이동 속도
    public int GetItemRange { get; private set; }     //아이템 획득 범위
    //public int ultimateID { get; private set; }
    //public int skillID { get; private set; }

    //public SkillData ultimateData;
    //public SkillData skillData;

    //Property setter를 사용할 경우 get이 같이 참조되어 setter를 따로 생성
    public void Set_Character_Name(string CharacterName) { this.CharacterName = CharacterName; }
    public void Set_Hp(int HP) { this.HP = HP; }
    public void Set_Attack(int Attack) { this.Attack = Attack; }
    public void Set_Cri_Ratio(int CriRatio) { this.CriRatio = CriRatio; }
    public void Set_Cri_Damage(int CriDamage) { this.CriDamage = CriDamage; }
    public void Set_Cool_Down(int CoolDown) { this.CoolDown = CoolDown; }
    public void Set_HP_Regen(int HPRegen) { this.HPRegen = HPRegen; }
    public void Set_Shield(int Shield) { this.Shield = Shield; }
    public void Set_Projectile_Add(int ProjectileAdd) { this.ProjectileAdd = ProjectileAdd; }
    public void Set_Move_Speed(int MoveSpeed) { this.MoveSpeed = MoveSpeed; }
    public void Set_Get_Item_Range(int GetItemRange) { this.GetItemRange = GetItemRange; }
    //public void SetUltimateID(int ultimateID) { this.ultimateID = ultimateID; }
    //public void SetskillID(int skillID) { this.skillID = skillID; }
    //public void SetUltimateData(SkillData ultimateData) { this.ultimateData = ultimateData; }
    //public void SetSkillData(SkillData skillData) { this.skillData = skillData; }
}