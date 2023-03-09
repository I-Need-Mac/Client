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

}
