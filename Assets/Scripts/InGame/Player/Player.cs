using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData playerData { get; private set; } = new PlayerData();

    //temp
    public int speed;

    #region MonoBehaviour Function
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    private void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal < 0)
        {
            moveVelocity.x = -1;
        }
        else if (horizontal > 0)
        {
            moveVelocity.x = 1;
        }
        if (vertical > 0)
        {
            moveVelocity.y = 1;
        }
        else if (vertical < 0)
        {
            moveVelocity.y = -1;
        }

        //transform.position += moveVelocity * playerData.moveSpeed * Time.deltaTime;
        transform.position += moveVelocity * speed * Time.deltaTime;
    }
}
