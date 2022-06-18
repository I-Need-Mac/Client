using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int limitTime;
    private Coroutine gameTimerCor;

    #region Setter
    //스테이지 제한 시간 설정
    public void SetLimitTime(int _limitTime)
    {
        limitTime = _limitTime;
    }
    #endregion

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
        float curTime = 0;

        while (curTime >= limitTime)
        {
            curTime += Time.deltaTime;

            yield return null;
        }

        //TODO :: 게임 오버 처리
    }
    #endregion
}
