using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterItem : MonoBehaviour
{
    private Transform target;
    private CircleCollider2D itemCollider;

    public bool isCollision { get; set; } = false;

    private void Awake()
    {
        itemCollider = GetComponent<CircleCollider2D>();
        itemCollider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        if (isCollision)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * 4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ItemCollider"))
        {
            DebugManager.Instance.PrintDebug("hi");
            isCollision = true;
            target = collision.gameObject.transform.parent.Find("Character").transform;
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            isCollision = false;
            gameObject.SetActive(false);
        }
    }
}
