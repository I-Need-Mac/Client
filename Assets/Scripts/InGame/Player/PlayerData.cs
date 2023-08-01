

using System;

public class PlayerData
{
    public string characterName { get; private set; }
    public string iconImage { get; private set; }

    public int hp
    {
        get { return originHp + modifyHp; }
        private set { }
    }

    public int currentHp { get; private set; }

    public float attack
    {
        get { return originAttack + modifyAttack; }
        private set { }
    }

    public int criRatio
    {
        get { return originCriRatio + modifyCriRatio; }
        private set { }
    }

    public float criDamage
    {
        get { return originCriDamage + modifyCriDamage; }
        private set { }
    }

    public int coolDown
    {
        get { return originCoolDown + modifyCoolDown; }
        private set { }
    }

    public int hpRegen
    {
        get { return originHpRegen + modifyHpRegen; }
        private set { }
    }

    public int shield { get; private set; }

    public int projectileAdd { get; private set; }

    public float moveSpeed
    {
        get { return originMoveSpeed + modifyMoveSpeed; }
        private set { }
    }

    public float getItemRange
    {
        get { return originGetItemRange + modifyGetItemRange; }
        private set { }
    }

    public string characterPrefabPath { get; private set; }

    public int expBuff { get; private set; }

    public int armor { get; private set; }

    public float projectileSize { get; private set; }

    public float projectileSplash { get; private set; }

    public float projectileSpeed { get; private set; }

    private int originHp;
    private int modifyHp;
    private float originAttack;
    private float modifyAttack;
    private int originCriRatio;
    private int modifyCriRatio;
    private float originCriDamage;
    private float modifyCriDamage;
    private int originCoolDown;
    private int modifyCoolDown;
    private int originHpRegen;
    private int modifyHpRegen;
    private float originMoveSpeed;
    private float modifyMoveSpeed;
    private float originGetItemRange;
    private float modifyGetItemRange;

    //Setter - Origin Data
    #region Setter
    public void SetCharacterName(string characterName)
    {
        this.characterName = characterName;
    }

    public void SetIconImage(string iconImage)
    {
        this.iconImage = iconImage;
    }

    public void SetHp(int hp)
    {
        this.originHp = hp;
        this.modifyHp = 0;
    }

    public void SetCurrentHp(int currentHp)
    {
        this.currentHp = currentHp;
    }

    public void SetAttack(float attack)
    {
        this.originAttack = attack/100;
        this.modifyAttack = 0.0f;
    }

    public void SetCriRatio(int criRatio)
    {
        this.originCriRatio = criRatio;
        this.modifyCriRatio = 0;
    }

    public void SetCriDamage(float criDamage)
    {
        this.originCriDamage = criDamage;
        this.modifyCriDamage = 0.0f;
    }

    public void SetCoolDown(int coolDown)
    {
        this.originCoolDown = coolDown;
        this.modifyCoolDown = 0;
    }

    public void SetHpRegen(int hpRegen)
    {
        this.originHpRegen = hpRegen;
        this.modifyHpRegen = 0;
    }

    public void SetShield(int shield)
    {
        this.shield = shield;
    }

    public void SetProjectileAdd(int projectileAdd)
    {
        this.projectileAdd = projectileAdd;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.originMoveSpeed = moveSpeed;
        this.modifyMoveSpeed = 0;
    }

    public void SetGetItemRange(float getItemRange)
    {
        this.originGetItemRange = getItemRange;
        this.modifyGetItemRange = 0;
    }

    public void SetExpBuff(int expBuff)
    {
        this.expBuff = expBuff;
    }

    public void SetArmor(int armor)
    {
        this.armor = armor;
    }

    public void SetProjectileSize(float projectileSize)
    {
        this.projectileSize = projectileSize;
    }

    public void SetProjectileSplash(float projectileSplash)
    {
        this.projectileSplash = projectileSplash;
    }

    public void SetProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }
    #endregion

    //Modifier - Modify Data
    #region Modifier
    public void HpModifier(int hp)
    {
        this.modifyHp += hp;
        if (this.currentHp > this.hp)
        {
            this.currentHp = this.hp;
        }
    }

    public void CurrentHpModifier(int currentHp)
    {
        this.currentHp += currentHp;
    }

    public void AttackModifier(float attack)
    {
        this.modifyAttack += attack;
    }

    public void CriRatioModifier(int criRatio)
    {
        this.modifyCriRatio += criRatio;
    }

    public void CriDamageModifier(float criDamage)
    {
        this.modifyCriDamage += criDamage;
    }

    public void CoolDownModifier(int coolDown)
    {
        this.modifyCoolDown += coolDown;
    }

    public void HpRegenModifier(int hpRegen)
    {
        this.modifyHpRegen += hpRegen;
    }

    public void ShieldModifier(int shield)
    {
        this.shield += shield;
    }

    public void ProjectileAddModifier(int projectileAdd)
    {
        this.projectileAdd += projectileAdd;
    }

    public void MoveSpeedModifier(float moveSpeed)
    {
        this.modifyMoveSpeed += moveSpeed;
    }

    public void GetItemRangeModifier(int getItemRange)
    {
        this.modifyGetItemRange += getItemRange;
    }

    public void ExpBuffModifier(int expBuff)
    {
        this.expBuff += expBuff;
    }

    public void ArmorModifier(int armor)
    {
        this.armor += armor;
    }

    public void ProjectileSizeModifier(float projectileSize)
    {
        this.projectileSize += projectileSize;
    }

    public void ProjectileSplashModifier(float projectileSplash)
    {
        this.projectileSize += projectileSplash;
    }

    public void ProjectileSpeedModifier(float projectileSpeed)
    {
        this.projectileSpeed += projectileSpeed;
    }
    #endregion

    public void HpRegen()
    {
        this.currentHp += this.hpRegen;
        if (this.currentHp > this.hp)
        {
            this.currentHp = this.hp;
        }
    }

    public int ExpBuff(int exp)
    {
        return (int)(exp * this.expBuff * 0.01f) + exp;
    }

    public int Armor(int damage)
    {
        return damage - (int)(damage * this.armor * 0.01f);
    }

    //[field: SerializeField] public string characterName { private set; get; }            //캐릭터 이름
    //[field: SerializeField] public int hp { private set; get; }                                 //체력
    //[field: SerializeField] public int currentHp { private set; get; }
    //[field: SerializeField] public float attack { private set; get; }                         //공격력
    //[field: SerializeField] public int criRatio { private set; get; }                       //크리티컬 확률
    //[field: SerializeField] public float criDamage { private set; get; }                      //크리티컬 데미지
    //[field: SerializeField] public int coolDown { private set; get; }                       //쿨타임 감소량
    //[field: SerializeField] public int hpRegen { private set; get; }                        //체젠량
    //[field: SerializeField] public int shield { private set; get; }                         //쉴드 개수
    //[field: SerializeField] public int projectileAdd { private set; get; }                 //투사체 증가 개수
    //[field: SerializeField] public int moveSpeed { private set; get; }                      //이동 속도
    //[field: SerializeField] public int getItemRange { private set; get; }                  //아이템 획득 범위
    //[field: SerializeField] public string characterPrefabPath { private set; get; }

    //private int originHp;
    //private float originAttack;
    //private int originCriRatio;
    //private float originCriDamage;
    //private int originCoolDown;
    //private int originHpRegen;
    //private int originMoveSpeed;
    //private int originGetItemRange;

    //#region Setter
    ////Property setter를 사용할 경우 get이 같이 참조되어 setter를 따로 생성
    //public void SetCharacterName(string characterName)
    //{
    //    this.characterName = characterName;
    //}

    //public void SetHp(int hp)
    //{
    //    this.originHp = hp;
    //    this.hp = this.originHp;
    //}

    //public void SetCurrentHp(int currentHp)
    //{
    //    this.currentHp = currentHp;
    //}

    //public void SetAttack(int attack)
    //{
    //    this.originAttack = attack * 0.01f;
    //    this.attack = this.originAttack;
    //}

    //public void SetCriRatio(int criRatio)
    //{
    //    this.originCriRatio = criRatio;
    //    this.criRatio = this.originCriRatio;
    //}

    //public void SetCriDamage(float criDamage)
    //{
    //    this.originCriDamage = criDamage;
    //    this.criDamage = this.originCriDamage;
    //}

    //public void SetCoolDown(int coolDown)
    //{
    //    this.originCoolDown = coolDown;
    //    this.coolDown = this.originCoolDown;
    //}

    //public void SetHpRegen(int hpRegen)
    //{
    //    this.originHpRegen = hpRegen;
    //    this.hpRegen = this.originHpRegen;
    //}

    //public void SetShield(int shield)
    //{
    //    this.shield = shield;
    //}

    //public void SetProjectileAdd(int projectileAdd)
    //{
    //    this.projectileAdd = projectileAdd;
    //}

    //public void SetMoveSpeed(int moveSpeed)
    //{
    //    this.originMoveSpeed = moveSpeed;
    //    this.moveSpeed = this.originMoveSpeed;
    //}

    //public void SetGetItemRange(int getItemRange)
    //{
    //    this.originGetItemRange = getItemRange;
    //    this.getItemRange = this.originGetItemRange;
    //}
    //#endregion

    //#region Modifier
    //public void HpModifier(int hp)
    //{
    //    this.hp += hp;
    //    if (this.currentHp > this.hp)
    //    {
    //        this.currentHp = this.hp;
    //    }
    //}

    //public void CurrentHpModifier(int currentHp)
    //{
    //    this.currentHp += currentHp;
    //}

    //public void AttackModifier(float attack)
    //{
    //    this.attack += attack;
    //}

    //public void CriRatioModifier(int criRatio)
    //{
    //    this.criRatio += criRatio;
    //}

    //public void CriDamageModifier(float criDamage)
    //{
    //    this.criDamage += criDamage;
    //}

    //public void CoolDownModifier(int coolDown)
    //{
    //    this.coolDown += coolDown;
    //}

    //public void HpRegenModifier(int hpRegen)
    //{
    //    this.hpRegen += hpRegen;
    //}

    //public void ShieldModifier(int shield)
    //{
    //    this.shield += shield;
    //}

    //public void ProjectileAddModifier(int projectileAdd)
    //{
    //    this.projectileAdd += projectileAdd;
    //}

    //public void MoveSpeedModifier(int moveSpeed)
    //{
    //    this.moveSpeed = this.originMoveSpeed;
    //    this.moveSpeed += moveSpeed;
    //}

    //public void GetItemRangeModifier(int getItemRange)
    //{
    //    this.getItemRange += getItemRange;
    //}
    //#endregion

}
