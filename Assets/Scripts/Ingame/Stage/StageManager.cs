using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int stageWidth;        //�������� ��
    private int stageHeight;       //�������� ����
    private int playerSponeX;      //�÷��̾� ���� ��ǥ X
    private int playerSponeY;      //�÷��̾� ���� ��ǥ Y
    private string monsterSponeID; //�� ���� ���̺�
    private int limitTime;         //���� �ð�
    private int limitAmount;       //�� ���� ��
    private int[] rewardID;        //���� ���̵�
    private int[] rewardAmount;    //���� ����
    private int introID;           //��Ʈ�� ID
    private int outroID;           //�ƿ�Ʈ�� ID

    [Header("--- Script ---")]
    [SerializeField] private GameManager GAME;
    [SerializeField] private Map map;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MonsterSpawner monsterSpawner;

    private void Awake()
    {
        //test
        StageInitSetting(10101);
    }

    public void StageInitSetting(int stageID)
    {
        if (LoadStageTable(stageID))
        {
            map.SetMapSize(stageWidth, stageHeight);

            playerManager.SetPlayerPos(new Vector2(playerSponeX, playerSponeY));

            monsterSpawner.LoadMonsterSpawnTable(monsterSponeID);
            monsterSpawner.SetLimitAmount(limitAmount);

            GAME.SetLimitTime(limitTime);
        }
    }

    private bool LoadStageTable(int stageID)
    {
        Debug.LogFormat("[LoadStageTable] Find {0} StageTable", stageID);
        //Dictionary<string, object> stageTableInfo = CSVReader.FindRead("StageTable", "StageID", stageID);

        //if (stageTableInfo != null)
        //{
        //    try
        //    {
        //        stageWidth = (int)stageTableInfo["StageWidth"];
        //        stageHeight = (int)stageTableInfo["StageHeight"];
        //        playerSponeX = (int)stageTableInfo["PlayerSponeX"];
        //        playerSponeY = (int)stageTableInfo["PlayerSponeY"];
        //        monsterSponeID = (string)stageTableInfo["MonsterSponeID"];
        //        limitTime = (int)stageTableInfo["LimitTime"];
        //        limitAmount = (int)stageTableInfo["LimitAmount"];
        //        rewardID = (int[])stageTableInfo["RewardID"];
        //        rewardAmount = (int[])stageTableInfo["RewardAmount"];
        //        introID = (int)stageTableInfo["IntroID"];
        //        outroID = (int)stageTableInfo["OutroID"];

        //        return true;
        //    }
        //    catch (InvalidCastException invalidCastEx)
        //    {
        //        Debug.LogError("[LoadStageTable] Data Type Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError("[LoadStageTable] " + ex.Message);

        //        //TODO :: �������� ���� ���� ó�� (�κ� ȭ������ �̵�)
        //    }
        //}
        //else
        //{
        //    Debug.LogError("[LoadStageTable] stageTableInfo is null.");

        //    //TODO :: �������� ���� ���� ó�� (�κ� ȭ������ �̵� or ���� ��˻�)
        //}

        return false;
    }
}
