using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerPoolManager playerPoolManager;
    [SerializeField] private string map;
    [SerializeField] private string player;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        string name = LoadMapManager.Instance.SceneNumberToMapName(10101);
        GameObject temp = LoadMapManager.Instance.LoadMapNameToMapObject(name);
        GameObject map = Instantiate(temp, transform);
        map.gameObject.transform.SetParent(transform.Find("MapGeneratePos").transform);
        map.gameObject.transform.localScale = Vector3.one;
        map.transform.position = new Vector3(map.transform.position.x, map.transform.position.y, 1);
        map.gameObject.SetActive(true);

        Player p = playerPoolManager.SpawnPlayer(transform.Find("PlayerSpawnPos").transform);
        p.transform.localScale = Vector3.one / 2;
        p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, 0);
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
