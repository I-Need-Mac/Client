using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData { get; private set; } = new MonsterData();
    private Player player;

    public bool isBoss { get; private set; }

    #region MonoBehaviour Function
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, monsterData.moveSpeed * Time.deltaTime);
    }
    #endregion

    public void LoadMonsterTable(int monsterID)
    {
        //Dictionary<string, object> monsterTableInfo = CSVReader.FindRead("MonsterTable", "MonsterID", monsterID);

        //if (monsterTableInfo != null)
        //{
        //    try
        //    {
        //        monsterData.SetMonsterName((string)monsterTableInfo["MonsterName"]);
        //        monsterData.SetHp((int)monsterTableInfo["HP"]);
        //        monsterData.SetAttack((int)monsterTableInfo["Attack"]);
        //        monsterData.SetMoveSpeed((int)monsterTableInfo["MoveSpeed"]);
        //        monsterData.SetAtkDistance((int)monsterTableInfo["AtkDistance"]);
        //        monsterData.SetSkillID((int)monsterTableInfo["SkillID"]);
        //        monsterData.SetExpDropRate((int)monsterTableInfo["ExpDropRate"]);
        //        monsterData.SetTreasureDropRate((int)monsterTableInfo["TreasureDropRate"]);
        //        monsterData.SetMoneyDropRate((int)monsterTableInfo["MoneyDropRate"]);
        //        monsterData.SetMonsterImage((string)monsterTableInfo["MonsterImage"]);
        //    }
        //    catch (InvalidCastException invalidCastEx)
        //    {
        //        Debug.LogError("[LoadMonsterTable] Data Type Error");

        //        gameObject.SetActive(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError("[LoadMonsterTable] " + ex.Message);

        //        gameObject.SetActive(false);
        //    }
        //}
        //else
        //{
        //    Debug.LogError("[LoadMonsterTable] stageTableInfo is null.");

        //    gameObject.SetActive(false);
        //}
    }
}