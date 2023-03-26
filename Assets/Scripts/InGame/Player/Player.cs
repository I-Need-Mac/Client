using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string skillId = "10101";
    [SerializeField] private int moveSpeed = 5;

    private Rigidbody2D playerRigidbody;
    private Vector2 playerDirection;

    private Transform character;
    private Transform shadow;
    private SpineAnimatorManager spineAnimatorManager;
    private bool isMovable = true;

    private int needExp;

    public PlayerManager playerManager { get; private set; }

    public Vector2 lookDirection { get; private set; } //바라보는 방향
    public int exp { get; private set; }
    public int level { get; private set; }

    #region Mono
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerDirection = Vector3.zero;
        lookDirection = Vector3.right;

        character = transform.Find("Character");
        shadow = transform.Find("Shadow");

        spineAnimatorManager = GetComponent<SpineAnimatorManager>();

        playerManager = transform.Find("PlayerManager").GetComponent<PlayerManager>();

        gameObject.tag = "Player";

        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
        level = 1;
    }

    private void Start()
    {
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
        TestFunction();
        PlayAnimations();
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
            spineAnimatorManager.SetSpineSpeed(moveSpeed);
            DebugManager.Instance.PrintDebug("MoveSpeed: {0}", moveSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            moveSpeed -= 1;
            spineAnimatorManager.SetSpineSpeed(moveSpeed);
            DebugManager.Instance.PrintDebug("MoveSpeed: {0}", moveSpeed);
        }
    }
    #endregion

    #region Movement, Animation
    private void PlayAnimations()
    {
        spineAnimatorManager.PlayAnimation("isMovable", isMovable);
    }

    //키보드 입력을 받아 방향을 결정하는 함수
    private void KeyDir()
    {
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        playerDirection.y = Input.GetAxisRaw("Vertical");

        lookDirection = playerDirection; //쳐다보는 방향 저장
    }

    private void Move()
    {
        spineAnimatorManager.SetDirection(character, playerDirection);
        spineAnimatorManager.SetDirection(shadow, playerDirection);
        playerRigidbody.velocity = playerDirection.normalized * moveSpeed;
        isMovable = playerRigidbody.velocity != Vector2.zero;
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
        ++level;
        exp -= needExp;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
    }
    #endregion

    #region Skill
    private void TempSkillSet(string str)
    {
        //playerManager.playerData.SetSkill(new Skill(str, this));
        //playerData.SetSkill(new Skill("10101", this)); //straight
        //playerData.SetSkill(new Skill("10300", this)); //satellite
        //playerData.SetSkill(new Skill("10500", this)); //boomerang
    }

    private void Fire()
    {
        TempSkillSet(skillId);
        for (int i = 0; i < playerManager.playerData.skills.Count; i++)
        {
            Skill skill = playerManager.playerData.skills[i];
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

    #region Collider

    #endregion

}
