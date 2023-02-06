using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D monsterRigidbody;
    private Vector3 monsterDirection;
    private Ray2D ray;
    private RaycastHit2D hitData;

    public MonsterData monsterData { get; private set; }
    public Vector3 lookDirection { get; private set; } //바라보는 방향

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
        //Tracking();
        Move();
    }

    private void Move()
    {
        //monsterRigidbody.velocity = Vector3.zero;
        //monsterRigidbody.angularVelocity = 0f;
        //float z = transform.position.z;
        //Vector3 vector = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 2f);
        //vector.z = z;
        //transform.position = vector;

        monsterRigidbody.velocity = (player.transform.position - transform.position).normalized * 3f;
    }

    private void Tracking()
    {
        monsterDirection = (player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.DrawRay(transform.position, monsterDirection * distance, Color.red);
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