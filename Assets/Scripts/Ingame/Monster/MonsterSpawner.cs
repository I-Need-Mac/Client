using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnLocation
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT
}

public class MonsterSpawner : MonoBehaviour
{
    private int limitAmount;

    #region Setter
    //몬스터 스폰 제한 수 설정
    public void SetLimitAmount(int limitAmount)
    {
        this.limitAmount = limitAmount;
    }
    #endregion

    //tableID 값의 몬스터 테이블 로드 후 불러온 정보 저장
    public void LoadMonsterSpawnTable(string tableID)
    {
        if (string.IsNullOrEmpty(tableID))
        {
            Debug.LogError("[LoadMonsterSpawnTable] tableID is null or empty.");
        }
        else
        {

        }
    }
}
