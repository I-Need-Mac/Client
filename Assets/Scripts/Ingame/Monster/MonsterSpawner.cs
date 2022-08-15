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
    //���� ���� ���� �� ����
    public void SetLimitAmount(int limitAmount)
    {
        this.limitAmount = limitAmount;
    }
    #endregion

    //tableID ���� ���� ���̺� �ε� �� �ҷ��� ���� ����
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
