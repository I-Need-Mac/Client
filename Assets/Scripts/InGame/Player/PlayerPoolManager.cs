using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : SingletonBehaviour<ProjectilePoolManager>
{
    [SerializeField] private PlayerPool playerPool;

    protected override void Awake()
    {
    }

    public Player SpawnPlayer(Transform transform)
    {
        Player player = playerPool.GetObject();
        player.transform.SetParent(transform);
        player.gameObject.SetActive(true);
        return player;
    }
}
