using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;
using BFM;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField] private PlayerPoolManager playerPoolManager;

    [SerializeField] private int mapId;
    [SerializeField] private int playerId;

    private GameObject map;
    private Player player;

    private float defaultScale;

    protected override void Awake()
    {
        defaultScale = float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "ImageMultiple", "ConfigValue")));
        playerPoolManager.playerId = playerId;
    }

    private void Start()
    {
        mapId = UIManager.Instance.selectStageID;
        playerId = UIManager.Instance.selectCharacterID;

        Spawn();
    }

    private void Spawn()
    {
        MapLoad(mapId);
        PlayerLoad(playerId);
        AssignLayerAndZ();
    }

    private void MapLoad(int mapId)
    {
        string name = LoadMapManager.Instance.SceneNumberToMapName(mapId);
        GameObject mapPrefab = LoadMapManager.Instance.LoadMapNameToMapObject(name);
        map = Instantiate(mapPrefab, transform);
        map.gameObject.transform.SetParent(transform.Find("MapGeneratePos").transform);
        map.gameObject.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        map.gameObject.SetActive(true);
    }

    private void PlayerLoad(int playerId)
    {
        playerPoolManager.playerId = playerId;
        player = playerPoolManager.SpawnPlayer(transform.Find("PlayerSpawnPos").transform);
        player.gameObject.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
    }

    private void AssignLayerAndZ()
    {
        RecursiveChild(player.transform, LayerConstant.SPAWNOBJECT);
        RecursiveChild(map.transform, LayerConstant.MAP);
    }

    private void RecursiveChild(Transform trans, LayerConstant layer)
    {
        trans.gameObject.layer = (int)layer;
        trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, (int)layer);

        foreach (Transform child in trans)
        {
            switch (child.name)
            {
                case "Camera":
                    RecursiveChild(child, LayerConstant.POISONFOG);
                    break;
                case "FieldStructure":
                    RecursiveChild(child, LayerConstant.OBSTACLE);
                    break;
                case "Top":
                    RecursiveChild(child, LayerConstant.OBSTACLE - 2);
                    break;
                default:
                    RecursiveChild(child, layer);
                    break;
            }
        }
    }

    //[field : Header("--- Object Pool ---")]
    //[field : SerializeField] public ProjectilePool projectilePool { get; private set; }
    //[field : SerializeField] public ObjectPool monsterPool { get; private set; }

    //[Header("--- Text ---")]
    //[SerializeField] private TMP_Text text_timer;

    //private int curTime;
    //private int limitTime;
    //private Coroutine gameTimerCor;

    //private void Awake()
    //{
    //    //monsterPool.AddObject();

    //    Init();
    //}

    //#region Setter
    ////�������� ���� �ð� ����
    //public void SetLimitTime(int limitTime)
    //{
    //    this.limitTime = limitTime;
    //}
    //#endregion

    //private void Init()
    //{
    //    text_timer.gameObject.SetActive(true);
    //    text_timer.text = "";
    //}

    //private void TimeOver()
    //{
    //    text_timer.text = "";
    //}

    //#region Coroutine
    //public void StartGameTimer()
    //{
    //    StopGameTimer();

    //    if (gameTimerCor == null)
    //    {
    //        gameTimerCor = StartCoroutine(GameTimer());
    //    }
    //}

    //public void StopGameTimer()
    //{
    //    if (gameTimerCor != null)
    //    {
    //        StopCoroutine(gameTimerCor);
    //    }
    //}

    //private IEnumerator GameTimer()
    //{
    //    var waitTime = new WaitForSeconds(1f);
    //    int m, s;

    //    curTime = limitTime / 1000;

    //    while (curTime > 0)
    //    {
    //        m = curTime / 60;
    //        s = curTime - (m * 60);

    //        text_timer.text = string.Format("{0:D2} : {1:D2}", m, s);

    //        curTime--;

    //        yield return waitTime;
    //    }

    //    TimeOver();
    //}
    //#endregion
}
