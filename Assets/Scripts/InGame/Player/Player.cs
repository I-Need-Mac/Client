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
        var fireDelay = new WaitForSeconds(skillData.coolTime); //�Ѿ� �߻� ������

        //���� ���� ��, ��� �ð� ���� �ߵ�
        if (!skillData.isEffect)
        {
            yield return fireDelay;
        }

        while (true)
        {
            try
            {
                //��Ȱ��ȭ �Ǿ� �ִ� ����ü ���
                Projectile projectile = GAME.projectilePool.GetProjectile(skillData.projectileType);
                //�θ� ������Ʈ ���� (Satellite)
                Transform parent = skillData.projectileType != ProjectileType.Satellite ? transform : GetEmptySateliteParent();

                if (parent != null)
                {
                    projectile.Init(); //����ü �ʱ�ȭ
                    projectile.SetIsMyProjectile(true); //�� �Ѿ� ���� bool �� True
                    Vector2 targetPos = GetTargetPos(skillData.skillTarget, skillData.atkDis); //Skill Target�� ���� ����ü �̵� ��ġ

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

    //ProjectileType�� ���� �Ѿ� �̵� ��ġ ���
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
