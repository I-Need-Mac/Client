using SKILLCONSTANT;

public class SkillData
{
    public int skillId { get; private set; }                        //스킬 id값
    public int coolTime { get; private set; }                       //스킬의 재사용 대기시간
    public int attackDistance { get; private set; }                 //스킬 사거리
    public int damage { get; private set; }                         //스킬 공격력
    public int skillEffectParam { get; private set; }               //스킬 이펙트 파라미터
    public bool skillCut { get; private set; }                      //필사기 연출 값
    public bool isEffect { get; private set; }                      //게임 시작 시 스킬 발동 여부
    public bool isUltimate { get; private set; }                    //스킬의 타입이 궁극기인지 여부
    public string name { get; private set; }                        //스킬 이름
    public string desc { get; private set; }                        //스킬 설명
    public string icon { get; private set; }                        //스킬 아이콘 데이터 연결값
    public string cutDire { get; private set; }                     //필살기 컷씬 연출 값
    public string skillImage { get; private set; }                  //스킬 발동 시 사용할 애니메이션 파일 이름
    public SKILL_EFFECT skillEffect { get; private set; }           //스킬 이펙트
    public SKILL_TARGET skillTarget { get; private set; }           //스킬 발동 대상
    public CALC_DAMAGE_TYPE calcDamageType { get; private set; }    //데미지 계산 방식
    public string skillPrefabPath { get; private set; }

    //가독성을 위해 투사체 전용은 따로 정리
    public int projectileCount { get; private set; }                //투사체 개수
    public int speed { get; private set; }                          //투사체 속도
    public int splashRange { get; private set; }                    //스플레쉬 범위 (원의 반지름)
    public int projectileSizeMulti { get; private set; }            //투사체 크기 배율
    public bool isPenetrate { get; private set; }                   //스킬의 관통 여부
    public PROJECTILE_TYPE projectileType { get; private set; }     //투사체 타입

    public void SetSkillId(int skillId) { this.skillId = skillId; }
    public void SetCoolTime(int coolTime) { this.coolTime = coolTime; }
    public void SetAttackDistance(int attackDistance) { this.attackDistance = attackDistance; }
    public void SetProjectileCount(int projectileCount) { this.projectileCount = projectileCount; }
    public void SetDamage(int damage) { this.damage = damage; }
    public void SetSpeed(int speed) { this.speed = speed; }
    public void SetSplashRange(int splashRange) { this.splashRange = splashRange; }
    public void SetProjectileSizeMulti(int projectileSizeMulti) { this.projectileSizeMulti = projectileSizeMulti; }
    public void SetSkillEffectParam(int skillEffectParam) { this.skillEffectParam = skillEffectParam; }
    public void SetSkillCut(bool skillCut) { this.skillCut = skillCut; }
    public void SetIsEffect(bool isEffect) { this.isEffect = isEffect; }
    public void SetIsUltimate(bool isUltimate) { this.isUltimate = isUltimate; }
    public void SetIsPenetrate(bool isPenetrate) { this.isPenetrate = isPenetrate; }
    public void SetName(string name) { this.name = name; }
    public void SetDesc(string desc) { this.desc = desc; }
    public void SetIcon(string icon) { this.icon = icon; }
    public void SetCutDire(string cutDire) { this.cutDire = cutDire; }
    public void SetSkillImage(string skillImage) { this.skillImage = skillImage; }
    public void SetSkillEffect(SKILL_EFFECT skillEffect) { this.skillEffect = skillEffect; }
    public void SetSkillTarget(SKILL_TARGET skillTarget) { this.skillTarget = skillTarget; }
    public void SetProjectileType(PROJECTILE_TYPE projectileType) { this.projectileType = projectileType; }
    public void SetCalcDamageType(CALC_DAMAGE_TYPE calcDamageType) { this.calcDamageType = calcDamageType; }
    public void SetSkillPrefabPath(string skillPrefabPath) { this.skillPrefabPath = skillPrefabPath; }
}
