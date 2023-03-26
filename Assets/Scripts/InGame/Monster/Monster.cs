
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Monster : MonoBehaviour
{
    [field: SerializeField] public int monsterId { get; private set; }

    private Player player;
    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;
    private bool isMovable = true;

    private SpineAnimatorManager spineAnimatorManager;

    public MonsterData monsterData { get; private set; } = new MonsterData();
    public Vector2 lookDirection { get; private set; } //바라보는 방향

    private void Awake()
    {
        spineAnimatorManager = GetComponent<SpineAnimatorManager>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        monsterDirection = Vector2.zero;
        lookDirection = Vector2.right;

        MonsterSetting(monsterId.ToString());
    }

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        PlayAnimations();
        Move();
    }

    private void Move()
    {
        spineAnimatorManager.SetDirection(transform, monsterDirection);

        isMovable = !(((Vector2)player.transform.position - (Vector2)transform.position).sqrMagnitude <= monsterData.atkDistance);
        if (isMovable)
        {
            monsterDirection = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;
        }
        else
        {
            monsterRigidbody.velocity = Vector2.zero;
        }
        
    }

    private void PlayAnimations()
    {
        spineAnimatorManager.PlayAnimation("isAttackable", !isMovable);
        spineAnimatorManager.PlayAnimation("isMovable", isMovable);
    }

    public void MonsterSetting(string monsterId)
    {
        Dictionary<string, Dictionary<string, object>> monsterTable = CSVReader.Read("MonsterTable");
        if (monsterTable.ContainsKey(monsterId))
        {
            Dictionary<string, object> table = monsterTable[monsterId];
            monsterData.SetMonsterName(Convert.ToString(table["MonsterName"]));
            monsterData.SetHp(Convert.ToInt32(table["HP"]));
            monsterData.SetSizeMultiple(float.Parse(Convert.ToString(table["SizeMultiple"])));
            monsterData.SetAttack(Convert.ToInt32(table["Attack"]));
            monsterData.SetMoveSpeed(Convert.ToInt32(table["MoveSpeed"]));
            monsterData.SetAtkSpeed(float.Parse(Convert.ToString(table["AtkSpeed"])));
            monsterData.SetViewDistance(Convert.ToInt32(table["ViewDistance"]));
            monsterData.SetAtkDistance(Convert.ToInt32(table["AtkDistance"]));
            monsterData.SetSkillID(Convert.ToInt32(table["SkillID"]));
            monsterData.SetGroupSource(Convert.ToString(table["GroupSource"]));
            monsterData.SetGroupSourceRate(Convert.ToInt32(table["GroupSourceRate"]));
            monsterData.SetMonsterPrefabPath(Convert.ToString(table["MonsterPrefabPath"]));
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
            monsterData.SetHp(monsterData.hp - player.playerManager.ReturnAttack());
            if (monsterData.hp <= 0)
            {
                DropItem();
                MonsterSpawner.Instance.DeSpawnMonster(this);
            }
        }
    }

}