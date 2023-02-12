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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    GameObject obj = collision.gameObject;
    //    if (obj.tag.Equals("Item"))
    //    {
    //        DebugManager.Instance.PrintDebug("item get!");
    //    }
    //}

}
