using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("--- Script ---")]
    public ObjectPool projectilePool;

    [Header("--- Text ---")]
    [SerializeField] private TMP_Text text_timer;

    private int curTime;
    private int limitTime;
    private Coroutine gameTimerCor;

    private void Awake()
    {
        projectilePool.AddObject();

        Init();
    }

    #region Setter
    //스테이지 제한 시간 설정
    public void SetLimitTime(int limitTime)
    {
        this.limitTime = limitTime;
    }
    #endregion

    private void Init()
    {
        text_timer.gameObject.SetActive(true);
        text_timer.text = "";
    }

    private void TimeOver()
    {
        text_timer.text = "";
    }

    #region Coroutine
    public void StartGameTimer()
    {
        StopGameTimer();

        if (gameTimerCor == null)
        {
            gameTimerCor = StartCoroutine(GameTimer());
        }
    }

    public void StopGameTimer()
    {
        if (gameTimerCor != null)
        {
            StopCoroutine(gameTimerCor);
        }
    }

    private IEnumerator GameTimer()
    {
        var waitTime = new WaitForSeconds(1f);
        int m, s;

        curTime = limitTime / 1000;

        while (curTime > 0)
        {
            m = curTime / 60;
            s = curTime - (m * 60);

            text_timer.text = string.Format("{0:D2} : {1:D2}", m, s);

            curTime--;

            yield return waitTime;
        }

        TimeOver();
    }
    #endregion
}
