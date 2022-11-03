using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string Character_Name { get; private set; }  //캐릭터 이름
    public int HP { get; private set; }                 //체력
    public int Attack { get; private set; }             //공격력
    public int Cri_Ratio { get; private set; }          //크리티컬 확률
    public int Cri_Damage { get; private set; }         //크리티컬 데미지
    public int Cool_Down { get; private set; }          //쿨타임 감소량
    public int HP_Regen { get; private set; }           //체젠량
    public int Shield { get; private set; }             //쉴드 개수
    public int Projectile_Add { get; private set; }     //투사체 증가 개수
    public int Move_Speed { get; private set; }         //이동 속도
    public int Get_Item_Range { get; private set; }     //아이템 획득 범위
    //public int ultimateID { get; private set; }
    //public int skillID { get; private set; }

    //public SkillData ultimateData;
    //public SkillData skillData;

    //Property setter를 사용할 경우 get이 같이 참조되어 setter를 따로 생성
    public void Set_Character_Name(string Character_Name) { this.Character_Name = Character_Name; }
    public void Set_Hp(int HP) { this.HP = HP; }
    public void Set_Attack(int Attack) { this.Attack = Attack; }
    public void Set_Cri_Ratio(int Cri_Ratio) { this.Cri_Ratio = Cri_Ratio; }
    public void Set_Cri_Damage(int Cri_Damage) { this.Cri_Damage = Cri_Damage; }
    public void Set_Cool_Down(int Cool_Down) { this.Cool_Down = Cool_Down; }
    public void Set_HP_Regen(int HP_Regen) { this.HP_Regen = HP_Regen; }
    public void Set_Shield(int Shield) { this.Shield = Shield; }
    public void Set_Projectile_Add(int Projectile_Add) { this.Projectile_Add = Projectile_Add; }
    public void Set_Move_Speed(int Move_Speed) { this.Move_Speed = Move_Speed; }
    public void Set_Get_Item_Range(int Get_Item_Range) { this.Get_Item_Range = Get_Item_Range; }
    //public void SetUltimateID(int ultimateID) { this.ultimateID = ultimateID; }
    //public void SetskillID(int skillID) { this.skillID = skillID; }
    //public void SetUltimateData(SkillData ultimateData) { this.ultimateData = ultimateData; }
    //public void SetSkillData(SkillData skillData) { this.skillData = skillData; }
}