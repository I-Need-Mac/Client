using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantuaryCircle : FieldStructure
{
    private bool isActive;

    protected override void Awake()
    {
        base.Awake();

        isActive = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
        {
            Player player = GetComponentInParent<Player>();
            if (player != null)
            {
                isActive = true;
                // 작업
            }
        }
    }
}
