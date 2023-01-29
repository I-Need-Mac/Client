using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;


public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerPoolManager playerPoolManager;

    private GameObject map;
    private Player player;

    private float defaultScale;

    private void Start()
    {
        defaultScale = float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "ImageMultiple", "ConfigValue")));
        Spawn();
    }

    private void Spawn()
    {
        MapLoad();
        PlayerLoad();
        AssignLayerAndZ();
    }

    private void MapLoad()
    {
        string name = LoadMapManager.Instance.SceneNumberToMapName(10101);
        GameObject mapPrefab = LoadMapManager.Instance.LoadMapNameToMapObject(name);
        map = Instantiate(mapPrefab, transform);
        map.gameObject.transform.SetParent(transform.Find("MapGeneratePos").transform);
        map.gameObject.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        map.gameObject.SetActive(true);
    }

    private void PlayerLoad()
    {
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
        trans.localPosition = new Vector3(trans.position.x, trans.position.y, (int)layer);

        foreach (Transform child in trans)
        {
            if (child.name.Equals("Camera"))
            {
                RecursiveChild(child, LayerConstant.POISONFOG);
                continue;
            }
            RecursiveChild(child, layer);
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
