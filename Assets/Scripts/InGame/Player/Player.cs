using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 5;

    private Rigidbody2D playerRigidbody;
    private Vector2 playerDirection;
    
    private Transform character;
    private Transform shadow;
    private SpineAnimatorManager spineAnimatorManager;
    private bool isMovable;
    private bool isHit;
    private WaitForSeconds invincibleTime;
    private SkeletonMecanim skeletonMecanim;

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

        spineAnimatorManager = GetComponent<SpineAnimatorManager>();

        playerManager = GetComponentInChildren<PlayerManager>();

        gameObject.tag = "Player";

        invincibleTime = new WaitForSeconds(float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "InvincibleTime", "ConfigValue"))));
        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
    }

    private void OnEnable()
    {
        level = 1;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
        isMovable = true;
        isHit = false;
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

        if (playerDirection != Vector2.zero)
        {
            lookDirection = playerDirection; //쳐다보는 방향 저장
        }
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
        exp -= needExp;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (++level + 1).ToString(), "NeedExp"));
        GameManager.Instance.playerUi.LevelTextChange(level);
        GameManager.Instance.playerUi.SkillSelectWindowOpen();
    }
    #endregion

    #region Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayerConstant.MONSTER && !isHit)
        {
            playerManager.weight.SetHp(playerManager.weight.hp - 10);
            StartCoroutine(Invincible());
        }
    }

    private IEnumerator Invincible()
    {
        RecursiveChild(transform, LayerConstant.INVINCIBLE);
        isHit = true;
        skeletonMecanim.skeleton.SetColor(Color.red);
        yield return invincibleTime;
        skeletonMecanim.skeleton.SetColor(Color.white);
        RecursiveChild(transform, LayerConstant.SPAWNOBJECT);
        isHit = false;
    }

    private void RecursiveChild(Transform trans, LayerConstant layer)
    {
        if (trans.name.Equals("Character"))
        {
            trans.tag = "Player";
        }
        trans.gameObject.layer = (int)layer;

        foreach (Transform child in trans)
        {
            switch (child.name)
            {
                case "Camera":
                    RecursiveChild(child, LayerConstant.POISONFOG);
                    break;
                case "FieldStructure":
                    RecursiveChild(child, LayerConstant.OBSTACLE);
                    break;
                case "Top":
                    RecursiveChild(child, LayerConstant.OBSTACLE - 2);
                    break;
                default:
                    RecursiveChild(child, layer);
                    break;
            }
        }
    }
    #endregion

}
