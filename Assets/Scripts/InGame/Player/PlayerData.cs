
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

    public float armor { get; private set; }

    #region PASSIVE
    public PassiveSet projectileSize { get; private set; } = new PassiveSet(0, 0);
    public PassiveSet projectileSpeed { get; private set; } = new PassiveSet(0, 0);
    public PassiveSet projectileSplash { get; private set; } = new PassiveSet(0, 0);
    public PassiveSet projectileDistance { get; private set; } = new PassiveSet(0, 0);
    public PassiveSet skillDamage { get; private set; } = new PassiveSet(0, 0);

    public void SetProjectileSize(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileSize = new PassiveSet(param, mode);
    }

    public void SetProjectileSpeed(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileSpeed= new PassiveSet(param, mode);
    }
    public void SetProjectileSplash(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileSplash = new PassiveSet(param, mode);
    }
    public void SetProjectileDistance(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileDistance = new PassiveSet(param, mode);
    }
    public void SetSkillDamage(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.skillDamage = new PassiveSet(param, mode);
    }

    //public float projectileSize { get; private set; }
    //public float projectileSpeed { get; private set; }
    //public float projectileSplash { get; private set; }
    //public float projectileDistance { get; private set; }
    //public float skillDamage { get; private set; }

    //public void SetProjectileSize(float projectileSize)
    //{
    //    this.projectileSize = projectileSize;
    //}

    //public void SetProjectileSpeed(float projectileSpeed)
    //{
    //    this.projectileSpeed = projectileSpeed;
    //}

    //public void SetProjectileSplash(float projectileSplash)
    //{
    //    this.projectileSplash = projectileSplash;
    //}

    //public void SetAttackDistance(float projectileDistance)
    //{
    //    this.projectileDistance = projectileDistance;
    //}

    //public void SetDamage(float skillDamage)
    //{
    //    this.skillDamage = skillDamage;
    //}

    //public void ProjectileSizeModifier(float projectileSize)
    //{
    //    this.projectileSize += projectileSize;
    //}

    //public void ProjectileSpeedModifier(float projectileSpeed)
    //{
    //    this.projectileSpeed += projectileSpeed;
    //}

    //public void ProjectileSplashModifier(float projectileSplash)
    //{
    //    this.projectileSize += projectileSplash;
    //}

    //public void ProjectileDistanceModifier(float projectileDistance)
    //{
    //    this.projectileDistance += projectileDistance;
    //}

    //public void SkillDamageModifier(float skillDamage)
    //{
    //    this.skillDamage += skillDamage;
    //}
    #endregion

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

    public void SetArmor(float armor)
    {
        this.armor = armor;
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
        return (int)(exp * (this.expBuff + 1.0f));
    }

    public int Armor(int damage)
    {
        return damage - (int)(damage * this.armor * 0.01f);
    }

}
