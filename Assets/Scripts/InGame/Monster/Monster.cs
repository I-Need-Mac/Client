
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Monster : MonoBehaviour
{
    [SerializeField] private int monsterId;

    private Player player;
    private Rigidbody2D monsterRigidbody;
    private Vector3 monsterDirection;

    private SpineManager anime;

    public MonsterData monsterData { get; private set; } = new MonsterData();
    public Vector3 lookDirection { get; private set; } //바라보는 방향

    private void Awake()
    {
        anime = GetComponent<SpineManager>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        monsterDirection = Vector3.zero;
        lookDirection = Vector3.right;
        transform.localScale = Vector3.one * float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "ImageMultiple", "ConfigValue")));
        MonsterSetting(Convert.ToString(monsterId));
    }

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        anime.SetCurrentAnimation();
    }

    private void FixedUpdate()
    {
        AnimationSetting();
        Move();
        //RayTest();
    }

    private void AnimationSetting()
    {
        anime.animationState = AnimationConstant.RUN;
        anime.SetDirection(monsterDirection);
    }

    private void Move()
    {
        monsterDirection = (player.transform.position - transform.position).normalized;
        monsterRigidbody.velocity = monsterDirection * 5f;
    }

    private void RayTest()
    {
        Debug.DrawRay(transform.position, monsterDirection * 3f);
        if (Physics2D.Raycast(transform.position, monsterDirection, 3f, 3))
        {
            DebugManager.Instance.PrintDebug("detect obstacle");
        }
    }

    private void MonsterSetting(string monsterId)
    {
        //Dictionary<string, Dictionary<string, object>> monsterTable = CSVReader.Read("MonsterTable");
        //if (monsterTable.ContainsKey(monsterId))
        //{
        //    Dictionary<string, object> table = monsterTable[monsterId];
        //    monsterData.SetMonsterName(Convert.ToString(table["MonsterName"]));
        //}
        monsterData.SetMonsterName(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "MonsterName")));
        monsterData.SetHp(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "HP")));
        monsterData.SetAttack(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "Attack")));
        monsterData.SetMoveSpeed(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "MoveSpeed")));
        monsterData.SetAtkSpeed(float.Parse(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "AtkSpeed"))));
        monsterData.SetViewDistance(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "ViewDistance")));
        monsterData.SetAtkDistance(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "AtkDistance")));
        monsterData.SetSkillID(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "SkillID")));
        monsterData.SetGroupSource(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "GroupSource")));
        monsterData.SetGroupSourceRate(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "GroupSourceRate")));
        monsterData.SetMonsterImage(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "MonsterImage")));
        monsterData.SetAttackType((AttackTypeConstant)Enum.Parse(typeof(AttackTypeConstant), Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "AttackType"))));
    }

    private void DropItem()
    {
        //Transform items = transform.Find("Item");
        //foreach (Transform item in items)
        //{
        //    GameObject dropItem = Instantiate(item.gameObject, dropItemField);
        //    dropItem.tag = "Item";
        //    dropItem.transform.position = transform.localPosition;
        //    dropItem.SetActive(true);
        //}
        ItemPoolManager.Instance.SpawnExpItem(1, 1, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("PlayerSkill"))
        {
            monsterData.SetHp(monsterData.hp - player.ReturnAttack());
            DebugManager.Instance.PrintDebug(monsterData.hp);
            if (monsterData.hp <= 0)
            {
                DropItem();
                MonsterSpawner.Instance.DeSpawnMonster(this);
            }
        }
    }

    //public float 

    //// 이동
    //if (sqrDistToPlayer < Mathf.Pow(viewDistance, 2))
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    //}

    //// 자동공격
    //if (Time.time > nextTimeAttack)
    //{
    //    sqrDistToPlayer = (player.transform.position - transform.position).sqrMagnitude;
    //    if (sqrDistToPlayer < Mathf.Pow(atkDistance, 2))
    //    {
    //        nextTimeAttack = Time.time + atkSpeed;
    //        Debug.Log(attack);
    //        // 플레이어 체력 mosterData.attack 만큼 감소
    //    }
    //}

}