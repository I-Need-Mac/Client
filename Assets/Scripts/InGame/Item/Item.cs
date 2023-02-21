using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float itemSpeed = 1f;

    private Player player;
    private Transform target;

    protected Collider2D itemCollider;
    protected SpriteRenderer render;

    public bool isCollision { get; set; } = false;

    private void Awake()
    {
        gameObject.tag = "Item";
    }

    private void FixedUpdate()
    {
        if (isCollision)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * itemSpeed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ItemCollider"))
        {
            isCollision = true;
            target = collision.gameObject.transform.parent.Find("Character").transform;
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            isCollision = false;
            gameObject.SetActive(false);
            GameManager.Instance.player.exp += 10;
            //DebugManager.Instance.PrintDebug("get exp: 10\ntotal exp: " + GameManager.Instance.player.exp);
        }
    }
}
