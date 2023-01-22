using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public string monsterID;
    private Dictionary<string, Dictionary<string, object>> monsterDataTable = new Dictionary<string, Dictionary<string, object>>();
    private Dictionary<string, object> monsterData = new Dictionary<string, object>();
    string monsterName;
    public int hp;
    int attack;
    public int moveSpeed;
    int atkSpeed;
    public int viewDistance;
    public int atkDistance;
    int skillID;
    string groupSource;
    int groupSourceRate;
    string monsterImage;
    string attackType;
    public GameObject player;

    private SpineManager anime;
    private Vector3 monsterDirection;

    // public bool isBoss { get; private set; }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        anime = GetComponent<SpineManager>();

        // ----------테스트용!!------------------
        monsterID = this.gameObject.name;
        monsterID = monsterID.Substring(0, 3);

        monsterDataTable = CSVReader.Read("MonsterTable");
        monsterData = monsterDataTable[monsterID];

        monsterName = (string)monsterData["MonsterName"];
        hp = int.Parse(monsterData["HP"].ToString());
        attack = int.Parse(monsterData["Attack"].ToString());
        moveSpeed = int.Parse(monsterData["MoveSpeed"].ToString());
        atkSpeed = int.Parse(monsterData["AtkSpeed"].ToString());
        viewDistance = int.Parse(monsterData["ViewDistance"].ToString());
        atkDistance = int.Parse(monsterData["AtkDistance"].ToString());
        skillID = int.Parse(monsterData["SkillID"].ToString());
        groupSource = (string)monsterData["GroupSource"];
        groupSourceRate = int.Parse(monsterData["GroupSourceRate"].ToString());
        monsterImage = (string)monsterData["MonsterImage"];
        attackType = (string)monsterData["AttackType"];
    }

    public float sqrDistToPlayer = 0 ;
    private float nextTimeAttack = 0 ;
    //public float 

    private void Update()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        sqrDistToPlayer = (player.transform.position - transform.position).sqrMagnitude;

        // 몬스터 방향 결정
        monsterDirection = player.transform.position - transform.position;
        anime.SetDirection(monsterDirection);
        if (monsterDirection != Vector3.zero)
        {
            //lookDirection = playerDirection;
            anime.animationState = AnimationConstant.RUN;
        }
        else
        {
            anime.animationState = AnimationConstant.IDLE;
        }

        anime.SetCurrentAnimation();
        
        
        // 이동
        if (sqrDistToPlayer < Mathf.Pow(viewDistance, 2))
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        // 자동공격
        if (Time.time > nextTimeAttack)
        {
            sqrDistToPlayer = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistToPlayer < Mathf.Pow(atkDistance, 2))
            {
                nextTimeAttack = Time.time + atkSpeed;
                Debug.Log(attack);
                // 플레이어 체력 mosterData.attack 만큼 감소
            }
        }

        //아이템 드롭
        if ( hp <= 0 )
        {
            if (Random.Range(0, 10000) <= groupSourceRate)
            {
                Debug.Log(groupSource);
            }
            // 임시로, 이후 스폰매니저에서 담당
            Destroy(this);
        }
        
    }

    /*
    public void LoadMonsterTable(string monsterID)
    {
        Dictionary<string, Dictionary<string, object>> monsterTable = CSVReader.Read("MonsterTable");
        Dictionary<string, object> monsterTableInfo = monsterTable[monsterID];

        if (monsterTableInfo != null)
        {
            try
            {
                monsterData.SetMonsterName((string)monsterTableInfo["MonsterName"]);
                monsterData.SetHp((int)monsterTableInfo["HP"]);
                monsterData.SetAttack((int)monsterTableInfo["Attack"]);
                monsterData.SetMoveSpeed((int)monsterTableInfo["MoveSpeed"]);
                monsterData.SetAtkSpeed((int)monsterTableInfo["AtkSpeed"]);
                monsterData.SetViewDistance((int)monsterTableInfo["ViewDistance"]);
                monsterData.SetAtkDistance((int)monsterTableInfo["AtkDistance"]);
                monsterData.SetSkillID((int)monsterTableInfo["SkillID"]);
                monsterData.SetDropID((int)monsterTableInfo["DropID"]);
                monsterData.SetMonsterImage((string)monsterTableInfo["MonsterImage"]);
                monsterData.SetAttackType((string)monsterTableInfo["AttackType"]);
            }
            catch (InvalidCastException invalidCastEx)
            {
                Debug.LogError("[LoadMonsterTable] Data Type Error");

                gameObject.SetActive(false);
            }
            catch (Exception ex)
            {
                Debug.LogError("[LoadMonsterTable] " + ex.Message);

                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("[LoadMonsterTable] stageTableInfo is null.");

            gameObject.SetActive(false);
        }
    }
    */
}