using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    private Player player;
    private CircleCollider2D itemCollider;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        gameObject.tag = "ItemCollider";
        itemCollider.isTrigger = true;
        itemCollider.radius = player.ReturnGetItemRange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Item"))
        {
            player.exp += 10;
            DebugManager.Instance.PrintDebug("get exp: 10\ntotal exp: " + player.exp);
        }
    }

}
