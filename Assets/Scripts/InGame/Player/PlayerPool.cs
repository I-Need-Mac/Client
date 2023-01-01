using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPool : ObjectPool<Player>
{
    public Player SpawnProjectile(float posX, float posY)
    {
        Player player = GetObject();
        player.transform.position = new Vector3(posX, posY, 0);
        player.gameObject.SetActive(true);
        return player;
    }
}
