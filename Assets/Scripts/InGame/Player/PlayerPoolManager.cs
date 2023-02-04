using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : SingletonBehaviour<PlayerPoolManager>
{
    [SerializeField] private PlayerPool playerPool;

    public int playerId { get; set; }

    protected override void Awake()
    {
    }

    public Player SpawnPlayer(Transform transform)
    {
        Player player = playerPool.GetObject();
        player.playerId = playerId;
        player.transform.SetParent(transform);
        player.gameObject.SetActive(true);
        return player;
    }

    public void DeSpawnPlayer(Player player)
    {
        playerPool.ReleaseObject(player);
    }
}
