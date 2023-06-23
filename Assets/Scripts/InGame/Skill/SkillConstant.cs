
namespace SKILLCONSTANT
{
    public enum SKILL_EFFECT
    {
        STUN,
        SLOW,
        KNOCKBACK,
        EXPLORE,
        MOVEUP,
        EXECUTE,
        SPAWNMOB,
        CHANGEFORM,
        BOUNCE,
        DRAIN,
        DELETE,
        RESTRAINT,
        PULL,
        METASTASIS,
        //아래는 패시브
        MAGNET,
        SHIELD,
        MOVESPEED,
        EXP,
        PROJECTILESIZE,
        PROJECTILECOUNT,
        PROJECTILESPLASH,
        PROJECTILESPEED,
        HP,
        HPREGEN,
        ATTACK,
        ATTACKSPEED,
        ARMOR,
    }

    public enum SKILL_TARGET
    {
        MELEE,
        FRONT,
        BACK,
        TOP,
        BOTTOM,
        RANDOM,
        BOSS,
        PLAYER,
        //ALL,
        LOWEST,
    }

    public enum CALC_MODE
    {
        PLUS,
        MULTI,
    }
}