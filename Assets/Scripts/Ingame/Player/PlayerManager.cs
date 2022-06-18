using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData { get; private set; }

    //플레이어 위치 변경
    public void SetPlayerPos(Vector2 newPos)
    {
        transform.position = newPos;
    }
}
