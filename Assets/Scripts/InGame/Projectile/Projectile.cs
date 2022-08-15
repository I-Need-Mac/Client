using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int speed;

    private ProjectileType projectileType;
    private bool isMyProjectile;

    private Vector2 movePos;
    private bool isMove;

    #region MonoBehaviour Function
    private void Update()
    {
        if (isMove)
        {
            Move();
        }
    }

    private void OnBecameInvisible()
    {
        Remove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMyProjectile)
        {
            if (collision.CompareTag("Enemy"))
            {
                Remove();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                Remove();
            }
        }
    }
    #endregion

    private void Move()
    {
        switch (projectileType)
        {
            case ProjectileType.Straight:
                transform.Translate(movePos * Time.deltaTime * speed);
                break;
        }
    }

    private void Remove()
    {
        isMove = false;
        movePos = Vector2.zero;

        gameObject.SetActive(false);
    }

    public void SetIsMyProjectile(bool isMyProjectile)
    {
        this.isMyProjectile = isMyProjectile;
    }

    public void Fire_Straight(Vector3 targetPos)
    {
        projectileType = ProjectileType.Straight;

        movePos = targetPos - transform.position;
        movePos.Normalize();

        isMove = true;
    }
}
