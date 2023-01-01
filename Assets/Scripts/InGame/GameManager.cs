using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerPool playerPool;
    [SerializeField] private string map;
    [SerializeField] private string player;

    

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
