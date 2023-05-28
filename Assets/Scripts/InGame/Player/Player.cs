using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 5;

    private Rigidbody2D playerRigidbody;
    private Vector2 playerDirection;
    
    
    private Transform shadow;
    private SpineManager spineManager;
    private WaitForSeconds invincibleTime;

    public Transform character { get; private set; }
    public PlayerManager playerManager { get; private set; }
    public Vector2 lookDirection { get; private set; } //바라보는 방향
    public int exp { get; private set; }
    public int level { get; private set; }
    public int needExp { get; private set; }

    #region Mono
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerDirection = Vector3.zero;
        lookDirection = Vector3.left;
        character = transform.Find("Character");
        shadow = transform.Find("Shadow");
        spineManager = GetComponent<SpineManager>();
        playerManager = GetComponentInChildren<PlayerManager>();
        gameObject.tag = "Player";
        invincibleTime = new WaitForSeconds(float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "InvincibleTime", "ConfigValue"))));
    }

    private void OnEnable()
    {
        level = 1;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
        //spineManager.SetAnimation("Idle", true);
    }

    /*
     *키보드 입력이랑 움직이는 부분은 안정성을 위해 분리시킴
     *Update -> 키보드 input
     *FixedUpdate -> movement
     */
    private void Update()
    {
        KeyDir();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #endregion

    #region Movement, Animation
    //키보드 입력을 받아 방향을 결정하는 함수
    private void KeyDir()
    {
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        playerDirection.y = Input.GetAxisRaw("Vertical");

        if (playerDirection != Vector2.zero)
        {
            lookDirection = playerDirection; //쳐다보는 방향 저장
        }
    }

    private void Move()
    {
        spineManager.SetDirection(character, playerDirection);
        spineManager.SetDirection(shadow, playerDirection);
        playerRigidbody.velocity = playerDirection.normalized * moveSpeed;
        if (playerRigidbody.velocity == Vector2.zero)
        {
            spineManager.SetAnimation("Idle", true);
        }
        else
        {
            spineManager.SetAnimation("Run", true, 0, playerManager.playerData.moveSpeed);
        }
    }
    #endregion

    #region Level
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
        exp -= needExp;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (++level + 1).ToString(), "NeedExp"));
        GameManager.Instance.playerUi.LevelTextChange(level);
        GameManager.Instance.playerUi.SkillSelectWindowOpen();
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.TryGetComponent(out Monster monster) && monster.isAttack)
        //{
        //    playerManager.weight.SetHp(playerManager.weight.hp - monster.monsterData.attack);
        //    StartCoroutine(Invincible());
        //}
        if (collision.TryGetComponent(out Monster monster))
        {
            DebugManager.Instance.PrintDebug("[COLtest]: ");
        }
    }

    public IEnumerator Invincible(Collider2D playerCollider)
    {
        spineManager.SetColor(Color.red);
        playerCollider.enabled = false;
        yield return invincibleTime;
        spineManager.SetColor(Color.white);
        playerCollider.enabled = true;
    }
    #endregion

}
