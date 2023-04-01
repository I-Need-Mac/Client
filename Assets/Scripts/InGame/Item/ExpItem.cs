using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    private float exp;

    private void Start()
    {
        itemCollider = GetComponent<CircleCollider2D>();
        itemCollider.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollision && collision.gameObject.tag.Equals("ItemCollider"))
        {
            isCollision = true;
            target = collision.transform.parent.Find("Character").transform;
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            isCollision = false;
            gameObject.SetActive(false);
            GameManager.Instance.player.GetExp(10);
            //DebugManager.Instance.PrintDebug("get exp: 10\ntotal exp: " + GameManager.Instance.player.exp);
        }
    }
}
