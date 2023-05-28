using BFM;
using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱글톤 사용
public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    private const float HP_REGEN_PER_SECOND = 1f;

    private int fraction;             //분율
    private int coolTimeConstant;     //재사용대기시간감소상수
    private int coolTimeCoefficient;  //재사용대기시간감소최대치조절계수
    private int criticalRatio;

    private RaycastHit2D raycast;
    private Vector2 pos;

    private Player player;
    private Collider2D playerCollider;

    public PlayerData playerData { get; private set; } = new PlayerData(); //플레이어의 데이터를 가지는 객체
    public PlayerData weight { get; private set; } = new PlayerData();     //증감치

    #region
    protected override void Awake()
    {
        PlayerSetting(FindCharacter(Convert.ToString(GameManager.Instance.GetPlayerId())));
        ConfigSetting();
        player = GetComponentInParent<Player>();
        playerCollider = GetComponent<Collider2D>();
        pos = new Vector2(0, 2);
    }

    //private void Start()
    //{
    //    StartCoroutine(HpRegeneration());
    //}

    //private void Update()
    //{
    //    raycast = Physics2D.CapsuleCast((Vector2)transform.position - pos, (Vector2)transform.position + pos, CapsuleDirection2D.Vertical, 30.0f, Vector2.zero, 0.0f, (int)LayerConstant.MONSTER);
    //}

    private void ConfigSetting()
    {
        fraction = Convert.ToInt32(CSVReader.Read("BattleConfig", "Fraction", "ConfigValue"));
        coolTimeConstant = Convert.ToInt32(CSVReader.Read("BattleConfig", "CoolTimeOffset", "ConfigValue"));
        coolTimeCoefficient = Convert.ToInt32(CSVReader.Read("BattleConfig", "CoolTimeMax", "ConfigValue"));
        criticalRatio = Convert.ToInt32(CSVReader.Read("BattleConfig", "CriticalRatio", "ConfigValue"));
    }

    //캐릭터에 스탯 부여
    public void PlayerSetting(Dictionary<string, object> characterData)
    {
        if (characterData == null)
        {
            //없는 캐릭터일 경우 에러메시지 송출
            Debug.LogError("존재하지 않는 캐릭터입니다");
            return;
        }

        playerData.SetCharacterName(Convert.ToString(characterData["CharacterName"]));
        playerData.SetHp(Convert.ToInt32(characterData["HP"]));
        playerData.SetAttack(Convert.ToInt32(characterData["Attack"]));
        playerData.SetCriRatio(Convert.ToInt32(characterData["CriRatio"]));
        playerData.SetCriDamage(Convert.ToInt32(characterData["CriDamage"]));
        playerData.SetCoolDown(Convert.ToInt32(characterData["CoolDown"]));
        playerData.SetHpRegen(Convert.ToInt32(characterData["HPRegen"]));
        playerData.SetShield(Convert.ToInt32(characterData["Shield"]));
        playerData.SetProjectileAdd(Convert.ToInt32(characterData["ProjectileAdd"]));
        playerData.SetMoveSpeed(Convert.ToInt32(characterData["MoveSpeed"]));
        playerData.SetGetItemRange(Convert.ToInt32(characterData["GetItemRange"]));
        //playerData.SetSkills();
    }

    //캐릭터 id와 일치하는 행(Dictionary)을 리턴
    private Dictionary<string, object> FindCharacter(string characterId)
    {
        Dictionary<string, Dictionary<string, object>> playerStatusTable = CSVReader.Read("CharacterTable");

        //foreach -> ContainsKey로 변경
        //가독성을 위함, 유의미한 성능차이 없음
        if (playerStatusTable.ContainsKey(characterId))
        {
            return playerStatusTable[characterId];
        }
        //찾는 캐릭터가 없을 경우 null 리턴
        return null;
    }
    #endregion

    /*스탯 증감 관련*/
    #region STATUS
    //increment는 버프+디버프 값 이하 status에 모두 동일하게 적용
    //버프, 디버프의 기본 수치는 0

    //체력
    public int ReturnHp()
    {
        return playerData.hp + weight.hp;
    }

    //공격력
    public int ReturnAttack()
    {
        return playerData.attack + weight.attack;
    }

    //크리티컬확률
    public int ReturnCriRatio()
    {
        return playerData.criRatio + weight.criRatio;
    }

    //크리티컬데미지 증감함수
    public int ReturnCriDamage()
    {
        return playerData.criDamage + weight.criDamage * criticalRatio;
    }

    //재사용대기시간 = 기존재사용대기시간*(재사용대기시간감소^2/(재사용대기시간감소^2+재사용대기시간감소상수))*재사용대기시간감소최대치조절계수/10000
    public float ReturnCoolDown()
    {
        return playerData.coolDown * ((float)Math.Pow(weight.coolDown, 2) / ((float)Math.Pow(weight.coolDown, 2) + coolTimeConstant)) * coolTimeCoefficient / fraction;
    }

    //체젠량
    public int ReturnHpRegen()
    {
        return playerData.hpRegen + weight.hpRegen;
    }

    //쉴드 개수
    public int ReturnShield()
    {
        return playerData.shield + weight.shield;
    }

    //투사체 증가 개수
    public int ReturnProjectileAdd()
    {
        return playerData.projectileAdd + weight.projectileAdd;
    }

    //이동속도
    public float ReturnMoveSpeed()
    {
        return playerData.moveSpeed * (1 + weight.moveSpeed);
    }

    //아이템 획득 범위
    public int ReturnGetItemRange()
    {
        return playerData.getItemRange + weight.getItemRange;
    }

    #endregion

    /*캐릭터 로직 관련*/
    #region Character Logic

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
            if (ReturnHp() < playerData.hp && ReturnHp() >= 0)
            {
                weight.SetHp(weight.hp + ReturnHpRegen());
            }
        }
    }

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
    public int TotalDamage(int skillDamage, CALC_DAMAGE_TYPE type)
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
            return ReturnCriDamage();
        }
        return originalDamage;
    }

    //쉴드 사용 함수
    //쉴드가 존재할경우 1감소시키고 데미지를 1로 반환
    //쉴드가 없을 경우 받은 데미지 그대로 리턴
    public int IsShield(int monsterDamage)
    {
        if (ReturnShield() > 0)
        {
            weight.SetShield(weight.shield - 1);
            return 1;
        }
        return monsterDamage;
    }
    #endregion

    /*몬스터 충돌*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster) && monster.isAttack)
        {
            DebugManager.Instance.PrintDebug("[충돌테스트]: 윽!");
            weight.SetHp(weight.hp - monster.monsterData.attack);
            StartCoroutine(player.Invincible(playerCollider));
        }
    }


}
