using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager GAME;
    [SerializeField] private Transform[] satelliteParents;

    public PlayerData playerData { get; private set; } = new PlayerData();

    private int moveDirX;
    private int moveDirY;

    private void Start()
    {
        StartCoroutine(Fire());
    }

    public void Init()
    {
        moveDirX = 1;
        moveDirY = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //x
        if (horizontal < 0)
        {
            moveVelocity.x = -1;
        }
        else if (horizontal > 0)
        {
            moveVelocity.x = 1;
        }
        //y
        if (vertical > 0)
        {
            moveVelocity.y = 1;
        }
        else if (vertical < 0)
        {
            moveVelocity.y = -1;
        }

        transform.position += moveVelocity * playerData.moveSpeed * Time.deltaTime;

        if (moveVelocity.x != 0 || moveVelocity.y != 0)
        {
            moveDirX = (int)moveVelocity.x;
            moveDirY = (int)moveVelocity.y;
        }
    }

    private IEnumerator Fire()
    {
        SkillData skillData = playerData.skillData;
        var fireDelay = new WaitForSeconds(skillData.coolTime); //총알 발사 딜레이

        //게임 시작 후, 대기 시간 이후 발동
        if (!skillData.isEffect)
        {
            yield return fireDelay;
        }

        while (true)
        {
            try
            {
                //비활성화 되어 있는 투사체 얻기
                Projectile projectile = GAME.projectilePool.GetProjectile(skillData.projectileType);
                //부모 오브젝트 설정 (Satellite)
                Transform parent = skillData.projectileType != ProjectileType.Satellite ? transform : GetEmptySateliteParent();

                if (parent != null)
                {
                    projectile.Init(); //투사체 초기화
                    projectile.SetIsMyProjectile(true); //내 총알 여부 bool 값 True
                    Vector2 targetPos = GetTargetPos(skillData.skillTarget, skillData.atkDis); //Skill Target에 따른 투사체 이동 위치

                    projectile.gameObject.SetActive(true);

                    projectile.Fire(parent, targetPos, playerData.skillData);
                }
            }
            catch (NullReferenceException nullEx)
            {
                Debug.LogError("[Player.Fire] " + nullEx.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError("[Player.Fire] " + ex.Message);
            }

            yield return fireDelay;
        }
    }

    //ProjectileType에 따른 총알 이동 위치 얻기
    private Vector2 GetTargetPos(SkillTarget skillTarget, int atkDis)
    {
        Vector2 targetPos = default(Vector2);

        switch (skillTarget)
        {
            case SkillTarget.Melee:
                targetPos = GameObject.FindGameObjectWithTag("Monster").transform.position;
                break;

            case SkillTarget.Front:
                targetPos = (Vector2)transform.position + (new Vector2(moveDirX, moveDirY) * atkDis);
                break;

            case SkillTarget.Back:
                targetPos = (Vector2)transform.position + (new Vector2(moveDirX, moveDirY) * -atkDis);
                break;

            case SkillTarget.Top:
                targetPos = (Vector2)transform.position + (Vector2.up * atkDis);
                break;

            case SkillTarget.Bottom:
                targetPos = (Vector2)transform.position + (Vector2.down * atkDis);
                break;

            case SkillTarget.RandomDrop:
                CameraManager cameraManager = CameraManager.Instance;

                if (cameraManager != null)
                {
                    targetPos = new Vector2(
                        Random.Range(cameraManager.Left, cameraManager.Right),
                        Random.Range(cameraManager.Top, cameraManager.Bottom)
                        );
                }
                else
                {
                    Debug.LogError("[GetTargetPos] cameraManager is null");

                    //TODO :: Add cameraManager object
                }

                break;

            case SkillTarget.Random:
                List<int> numList = new List<int>() { -1, -1, 0, 1, 1 };
                int index = 0;

                index = Random.Range(0, numList.Count);
                float x = numList[index];
                numList.RemoveAt(index);

                index = Random.Range(0, numList.Count);
                float y = numList[index];

                targetPos = new Vector2(x, y) * atkDis;
                break;

            case SkillTarget.Near:
                targetPos = transform.position;
                break;

            case SkillTarget.Boss:
                Monster[] monsters = FindObjectsOfType<Monster>();

                foreach (var monster in monsters)
                {
                    if (monster.isBoss)
                    {
                        targetPos = monster.transform.position;
                        break;
                    }
                }

                break;
        }

        return targetPos;
    }

    private Transform GetEmptySateliteParent()
    {
        for (int i = 0; i < satelliteParents.Length; i++)
        {
            if (satelliteParents[i].childCount == 0)
            {
                return satelliteParents[i];
            }
        }

        return null;
    }
}
