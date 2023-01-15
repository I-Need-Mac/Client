using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float HP_REGEN_PER_SECOND = 1f;
    private const float PER = 10000f; //분율 수치 ex)100 -> 백분율, 1000 -> 천분율, 10000 -> 만분율
    private const string CONFIG_VALUE = "ConfigValue";

    [SerializeField] private string skillId = "10101";
    [SerializeField] private int moveSpeed = 5;

    private Rigidbody2D playerRigidbody;
    private Vector3 playerDirection;
    private float coolTimeConstant;     //재사용대기시간감소상수
    private float coolTimeCoefficient;  //재사용대기시간감소최대치조절계수

    private SpineManager anime;
    /*
     *캐릭터 스탯의 경우
     *기본값들의 변화는 생기지 않음 -> 혼을 이용해서 게임 스타트 초기에만 변화를 주는 형태
     *그러므로 기본 값들에 변화를 주지 않기 위해서 임시 변수를 생성해야함
     */
    public int hp;
    private int currentHp;
    private int attack;
    private int criRatio;
    private int criDamage;
    private float coolDown;
    private int hpRegen;
    public int shield;
    private int projectileAdd;
    private int getItemRange;

    public PlayerData playerData { get; private set; } = new PlayerData();
    public Vector3 lookDirection { get; private set; } //바라보는 방향

    /*Unity Mono*/
    #region Mono
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerDirection = Vector3.zero;
        lookDirection = Vector3.right;

        anime = GetComponent<SpineManager>();

        coolTimeConstant = findConstant("CoolTimeConstant");
        coolTimeCoefficient = findConstant("CoolTimeCoefficient");
    }

    //playerData의 경우 Awake단계에서 PlayerManager로 인한 데이터 셋팅이 이루어지지 않으므로 Start에 배치
    private void Start()
    {
        statusSetting(playerData);
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
    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    /*유틸*/
    #region Util
    //상수값 읽어오는 함수
    private int findConstant(string constantName)
    {
        Dictionary<string, Dictionary<string, object>> configTable = CSVReader.Read("Battleconfig");

        try
        {
            return Convert.ToInt32(configTable[constantName][CONFIG_VALUE]);
        }
        catch (Exception e)
        {
            DebugManager.Instance.PrintDebug("[ERROR] 상수 이름을 다시 확인해 주세요");
            return 0;
        }
    }

    private void statusSetting(PlayerData playerData)
    {
        hp = playerData.hp;
        currentHp = hp;
        attack = playerData.attack;
        criRatio = playerData.criRatio;
        criDamage = playerData.criDamage;
        coolDown = playerData.coolDown;
        hpRegen = playerData.hpRegen;
        shield = playerData.shield;
        projectileAdd = playerData.projectileAdd;
        moveSpeed = playerData.moveSpeed;
        getItemRange = playerData.getItemRange;
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
    #region Modify STATUS
    //increment는 버프+디버프 값 이하 status에 모두 동일하게 적용
    //버프, 디버프의 기본 수치는 0

    //체력 증감 함수
    public void ModifyHp(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        hp += increment;
    }

    //공격력 증감 함수
    private void ModifyAttack(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        attack += increment;
    }

    //크리티컬확률 증감 함수
    private void ModifyCriRatio(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        criRatio += increment;
    }

    //크리티컬데미지 증감함수
    private void ModifyCriDamage(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        criDamage += increment;
    }

    //재사용대기시간 증감 함수
    //재사용대기시간 = 기존재사용대기시간*(재사용대기시간감소^2/(재사용대기시간감소^2+재사용대기시간감소상수))*재사용대기시간감소최대치조절계수/10000
    private void ModifyCoolDown(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        float calculateCoolDown = coolDown * (increment * increment / (increment * increment + coolTimeConstant)) * coolTimeCoefficient / PER;
        coolDown = calculateCoolDown;
    }

    //체젠량 증감 함수
    private void ModifyHpRegen(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        hpRegen += increment;
    }

    //쉴드개수 증감함수
    private void ModifyShield(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        shield += increment;
    }

    //투사체 개수 증감 함수
    private void ModifyProjectileAdd(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        projectileAdd += increment;
    }

    //이동속도 증감 함수
    private void ModifyMoveSpeed(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        moveSpeed *= 1 + (buff - deBuff);
    }

    //아이템획득범위 증감함수
    private void ModifyGetItemRange(int buff = 0, int deBuff = 0)
    {
        int increment = buff + deBuff;
        getItemRange += increment;
    }

    #endregion


    /*캐릭터 로직 관련*/
    #region Character Logic
    //크리티컬 판별 함수
    private bool IsCritical()
    {
        return UnityEngine.Random.Range(0f, 1f) <= (criRatio / PER);
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
            originalDamage = attack + skillDamage;
        }
        else
        {
            originalDamage = attack * skillDamage;
        }

        //크리티컬 체크
        if (IsCritical())
        {
            return (int)(originalDamage * (1 + criDamage / PER)); //소수점 버림
        }
        return originalDamage;
    }

    //쉴드 사용 함수
    //쉴드가 존재할경우 1감소시키고 데미지를 1로 반환
    //쉴드가 없을 경우 받은 데미지 그대로 리턴
    private int IsShield(int monsterDamage)
    {
        if(shield > 0)
        {
            --shield;
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
            if (currentHp < playerData.hp)
            {
                currentHp += hpRegen;
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

}
