using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public int skillID;             //스킬 아이디
    public int coolTime;            //쿨타임
    public int atkDis;              //사거리
    public int projectileCount;     //투사체 개수
    public int damage;              //피해
    public int speed;               //투사체 스피드
    public int splashRange;         //폭발 반지름
    public int projectileSizeMulti; //투사체 사이즈 배율
    public int skillEffectParam;    //스킬 효과 파람

    public string name;     //스킬 이름
    public string desc;     //스킬 설명
    public string icon;     //아이콘
    public string cutDire;  //스킬 컷씬 경로
    public string skillImg; //스킬 이미지

    public bool skillCut;    //스킬 컷씬 유무
    public bool isEffect;    //스킬 바로 시전 여부
    public bool isUltimate;  //궁극기 여부
    public bool isSplash;    //스플래쉬 여부
    public bool isPenetrate; //관통 여부
    
    public SkillEffect skillEffect;       //스킬 효과
    public SkillTarget skillTarget;       //스킬 발사 타겟
    public ProjectileType projectileType; //스킬 날아가는 형태
}
