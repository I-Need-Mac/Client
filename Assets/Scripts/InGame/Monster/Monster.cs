
using Spine.Unity;
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

    private SpineAnimatorManager spineAnimatorManager;

    public MonsterData monsterData { get; private set; } = new MonsterData();
    public Vector3 lookDirection { get; private set; } //바라보는 방향

    private void Awake()
    {
        spineAnimatorManager = GetComponent<SpineAnimatorManager>();
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
        PlayAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        spineAnimatorManager.SetDirection(transform, monsterDirection);

        monsterDirection = (player.transform.position - transform.position).normalized;
        monsterRigidbody.velocity = monsterDirection * 5f;
    }

    private void PlayAnimation()
    {
        spineAnimatorManager.animator.SetBool("isAttack", ((Vector2)(player.transform.position - transform.position)).sqrMagnitude <= monsterData.atkDistance);
        spineAnimatorManager.animator.SetBool("isMovable", monsterRigidbody.velocity != Vector2.zero);
    }

    private void MonsterSetting(string monsterId)
    {
        Dictionary<string, Dictionary<string, object>> monsterTable = CSVReader.Read("MonsterTable");
        if (monsterTable.ContainsKey(monsterId))
        {
            Dictionary<string, object> table = monsterTable[monsterId];
            monsterData.SetMonsterName(Convert.ToString(table["MonsterName"]));
            monsterData.SetHp(Convert.ToInt32(table["HP"]));
            monsterData.SetAttack(Convert.ToInt32(table["Attack"]));
            monsterData.SetMoveSpeed(Convert.ToInt32(table["MoveSpeed"]));
            monsterData.SetAtkSpeed(float.Parse(Convert.ToString(table["AtkSpeed"])));
            monsterData.SetViewDistance(Convert.ToInt32(table["ViewDistance"]));
            monsterData.SetAtkDistance(Convert.ToInt32(table["AtkDistance"]));
            monsterData.SetSkillID(Convert.ToInt32(table["SkillID"]));
            monsterData.SetGroupSource(Convert.ToString(table["GroupSource"]));
            monsterData.SetGroupSourceRate(Convert.ToInt32(table["GroupSourceRate"]));
            monsterData.SetMonsterImage(Convert.ToString(table["MonsterImage"]));
            monsterData.SetAttackType((AttackTypeConstant)Enum.Parse(typeof(AttackTypeConstant), Convert.ToString(table["AttackType"])));
        }
        //monsterData.SetMonsterName(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "MonsterName")));
        //monsterData.SetHp(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "HP")));
        //monsterData.SetAttack(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "Attack")));
        //monsterData.SetMoveSpeed(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "MoveSpeed")));
        //monsterData.SetAtkSpeed(float.Parse(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "AtkSpeed"))));
        //monsterData.SetViewDistance(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "ViewDistance")));
        //monsterData.SetAtkDistance(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "AtkDistance")));
        //monsterData.SetSkillID(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "SkillID")));
        //monsterData.SetGroupSource(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "GroupSource")));
        //monsterData.SetGroupSourceRate(Convert.ToInt32(CSVReader.Read("MonsterTable", monsterId, "GroupSourceRate")));
        //monsterData.SetMonsterImage(Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "MonsterImage")));
        //monsterData.SetAttackType((AttackTypeConstant)Enum.Parse(typeof(AttackTypeConstant), Convert.ToString(CSVReader.Read("MonsterTable", monsterId, "AttackType"))));
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
        ItemPoolManager.Instance.SpawnExpItem(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("PlayerSkill"))
        {
            monsterData.SetHp(monsterData.hp - player.ReturnAttack());
            if (monsterData.hp <= 0)
            {
                DropItem();
                MonsterSpawner.Instance.DeSpawnMonster(this);
            }
        }
    }

}