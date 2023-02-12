
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D monsterRigidbody;
    private Vector3 monsterDirection;

    public MonsterData monsterData { get; private set; }
    public Vector3 lookDirection { get; private set; } //바라보는 방향

    private Ray2D ray;
    private RaycastHit2D hitData;

    private void Awake()
    {
        monsterData = new MonsterData();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        monsterDirection = Vector3.zero;
        lookDirection = Vector3.right;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        Move();
        RayTest();
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