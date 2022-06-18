using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int limitTime;
    private Coroutine gameTimerCor;

    #region Setter
    //�������� ���� �ð� ����
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

        //TODO :: ���� ���� ó��
    }
    #endregion
}
