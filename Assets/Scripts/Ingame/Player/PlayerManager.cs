using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData { get; private set; }

    //�÷��̾� ��ġ ����
    public void SetPlayerPos(Vector2 newPos)
    {
        transform.position = newPos;
    }
}
