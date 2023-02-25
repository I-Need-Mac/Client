using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float HP_REGEN_PER_SECOND = 1f;

    [SerializeField] private string skillId = "10101";
    [SerializeField] private int moveSpeed = 5;

    private Rigidbody2D playerRigidbody;
    private Vector3 playerDirection;

    private int fraction;             //분율
    private int coolTimeConstant;     //재사용대기시간감소상수
    private int coolTimeCoefficient;  //재사용대기시간감소최대치조절계수
    private int criticalRatio;

    private SpineManager anime;
    private PlayerManager playerManager;

    private PlayerData weight = new PlayerData(); //증감치

    private int needExp;

    public PlayerData playerData { get; private set; } = new PlayerData();
    public Vector3 lookDirection { get; private set; } //바라보는 방향
    //public int playerId { get; set; }
    public int exp { get; private set; }
    public int level { get; private set; }

    /*Unity Mono*/
    #region Mono
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerDirection = Vector3.zero;
        lookDirection = Vector3.right;

        anime = GetComponent<SpineManager>();
        playerManager = transform.Find("PlayerManager").GetComponent<PlayerManager>();

        ConfigSetting();

        gameObject.tag = "Player";

        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
        level = 1;
    }

    private void ConfigSetting()
    {
        fraction = Convert.ToInt32(CSVReader.Read("BattleConfig", "Fraction", "ConfigValue"));
        coolTimeConstant = Convert.ToInt32(CSVReader.Read("BattleConfig", "CoolTimeOffset", "ConfigValue"));
        coolTimeCoefficient = Convert.ToInt32(CSVReader.Read("BattleConfig", "CoolTimeMax", "ConfigValue"));
        criticalRatio = Convert.ToInt32(CSVReader.Read("BattleConfig", "CriticalRatio", "ConfigValue"));
    }

    //playerData의 경우 Awake단계에서 PlayerManager로 인한 데이터 셋팅이 이루어지지 않으므로 Start에 배치
    private void Start()
    {
        StartCoroutine(HpRegeneration());
        Fire();
    }


    /*
     *키보드 입력이랑 움직이는 부분은 안정성을 위해 분리시킴
     *Update -> 키보드 input
     *FixedUpdate -> movement
     */
    private void Update()
    {
        KeyDir();
        anime.SetCurrentAnimation();
        TestFunction();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void TestFunction()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            moveSpeed += 1;
            anime.SetSpineSpeed(moveSpeed);
            DebugManager.Instance.PrintDebug("MoveSpeed: {0}", moveSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            moveSpeed -= 1;
            anime.SetSpineSpeed(moveSpeed);
            DebugManager.Instance.PrintDebug("MoveSpeed: {0}", moveSpeed);
        }
    }
    #endregion

    /*키보드 입력 및 움직임 관련*/
    #region key input & movement
    //키보드 입력을 받아 방향을 결정하는 함수
    private void KeyDir()
    {
        //left, right
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        //up, down
        playerDirection.y = Input.GetAxisRaw("Vertical");

        anime.SetDirection(playerDirection);

        if (playerDirection != Vector3.zero)
        {
            lookDirection = playerDirection; //쳐다보는 방향 저장
            anime.animationState = AnimationConstant.RUN; //움직이는 중
        }
        else
        {
            anime.animationState = AnimationConstant.IDLE;
        }

    }

    //리지드바디의 MovePosition을 이용해 움직임을 구현
    private void Move()
    {
        //playerRigidbody.MovePosition((Vector3)playerRigidbody.position + (playerDirection * moveSpeed * Time.fixedDeltaTime));
        playerRigidbody.velocity = playerDirection.normalized * moveSpeed;
    }
    #endregion

    /*스탯 증감 관련*/
    #region STATUS
    //increment는 버프+디버프 값 이하 status에 모두 동일하게 적용
    //버프, 디버프의 기본 수치는 0

    //체력
    public int ReturnHp()
    {
        return playerManager.playerData.hp + weight.hp;
    }

    //공격력
    public int ReturnAttack()
    {
        return playerManager.playerData.attack + weight.attack;
    }

    //크리티컬확률
    public int ReturnCriRatio()
    {
        return playerManager.playerData.criRatio + weight.criRatio;
    }

    //크리티컬데미지 증감함수
    public int ReturnCriDamage()
    {
        return playerManager.playerData.criDamage + weight.criDamage * criticalRatio;
    }

    //재사용대기시간 = 기존재사용대기시간*(재사용대기시간감소^2/(재사용대기시간감소^2+재사용대기시간감소상수))*재사용대기시간감소최대치조절계수/10000
    public float ReturnCoolDown()
    {
        return playerManager.playerData.coolDown * ((float)Math.Pow(weight.coolDown, 2) / ((float)Math.Pow(weight.coolDown, 2) + coolTimeConstant)) * coolTimeCoefficient / fraction;
    }

    //체젠량
    public int ReturnHpRegen()
    {
        return playerManager.playerData.hpRegen + weight.hpRegen;
    }

    //쉴드 개수
    public int ReturnShield()
    {
        return playerManager.playerData.shield + weight.shield;
    }

    //투사체 증가 개수
    public int ReturnProjectileAdd()
    {
        return playerManager.playerData.projectileAdd + weight.projectileAdd;
    }

    //이동속도
    public float ReturnMoveSpeed()
    {
        return playerManager.playerData.moveSpeed * (1 + weight.moveSpeed);
    }

    //아이템 획득 범위
    public int ReturnGetItemRange()
    {
        return playerManager.playerData.getItemRange + weight.getItemRange;
    }

    #endregion

    /*캐릭터 로직 관련*/
    #region Character Logic
    //크리티컬 판별 함수
    private bool IsCritical()
    {
        return UnityEngine.Random.Range(0f, 1f) <= (ReturnCriRatio() / fraction);
    }

    //최종적으로 몬스터에게 가하는 데미지 계산 함수
    //스킬 데미지 계산 방식에 따라 따로 계산
    //오리지널데미지 = 공격력 + or * 스킬피해
    //크리티컬데미지 = 오리지널데미지 * 크리티컬데미지
    //일단 스킬피해 제외하고 구현
    private int TotalDamage(int skillDamage, CALC_DAMAGE_TYPE type)
    {
        int originalDamage;
        if (type == CALC_DAMAGE_TYPE.PLUS)
        {
            originalDamage = ReturnAttack() + skillDamage;
        }
        else
        {
            originalDamage = ReturnAttack() * skillDamage;
        }

        //크리티컬 체크
        if (IsCritical())
        {
            return (int)(originalDamage * (1 + ReturnCriDamage() / fraction)); //소수점 버림
        }
        return originalDamage;
    }

    //쉴드 사용 함수
    //쉴드가 존재할경우 1감소시키고 데미지를 1로 반환
    //쉴드가 없을 경우 받은 데미지 그대로 리턴
    private int IsShield(int monsterDamage)
    {
        if(ReturnShield() > 0)
        {
            weight.SetShield(weight.shield - 1);
            return 1;
        }
        return monsterDamage;
    }

    //체젠 함수
    //기본적으로 가지고 있는(get으로 가져올 수 있는) hp를 최대 체력이라 지정
    //현재 체력은 currentHp로 따로 빼서 사용
    private IEnumerator HpRegeneration()
    {
        while (true)
        {
            //정해진 초(HP_REGEN_PER_SECOND)마다 실행
            yield return new WaitForSeconds(HP_REGEN_PER_SECOND);

            //최대 체력보다 현재 체력이 낮을 때 체젠량만큼 회복
            if (ReturnHp() < playerManager.playerData.hp && ReturnHp() >= 0)
            {
                weight.SetHp(weight.hp + ReturnHpRegen());
            }
        }
    }

    ////충돌시작
    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //}

    ////충돌 중
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    Debug.Log(collision.ToString());
    //    if (currentHp <= 0)
    //    {
    //        DebugManager.Instance.PrintDebug("죽었습니다");
    //        return;
    //    }
    //    currentHp -= 10;
    //    DebugManager.Instance.PrintDebug("현재체력: {0}", currentHp);
    //}
    #endregion

    /*캐릭터 레벨 관련*/
    #region
    public void GetExp(int exp)
    {
        this.exp += exp;

        if (this.exp >= needExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        ++level;
        exp -= needExp;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
    }
    #endregion

    /*스킬 관련*/
    #region Skill
    private void TempSkillSet(string str)
    {
        playerData.SetSkill(new Skill(str, this));
        //playerData.SetSkill(new Skill("10101", this)); //straight
        //playerData.SetSkill(new Skill("10300", this)); //satellite
        //playerData.SetSkill(new Skill("10500", this)); //boomerang
    }

    private void Fire()
    {
        TempSkillSet(skillId);
        for (int i = 0; i < playerData.skills.Count; i++)
        {
            Skill skill = playerData.skills[i];
            switch (skill.skillData.projectileType)
            {
                case PROJECTILE_TYPE.SATELLITE:
                    StartCoroutine(skill.SatelliteSkill());
                    break;
                case PROJECTILE_TYPE.PROTECT:
                    skill.ProtectSkill();
                    break;
                default:
                    StartCoroutine(skill.ShootSkill());
                    break;
            }
        }
    }
    #endregion

    /*콜라이더 관련*/
    #region

    #endregion

}
