using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int stageWidth;        //스테이지 폭
    private int stageHeight;       //스테이지 높이
    private int playerSponeX;      //플레이어 스폰 좌표 X
    private int playerSponeY;      //플레이어 스폰 좌표 Y
    private string monsterSponeID; //몹 스폰 테이블
    private int limitTime;         //제한 시간
    private int limitAmount;       //몹 제한 수
    private int[] rewardID;        //보상 아이디
    private int[] rewardAmount;    //보상 개수
    private int introID;           //인트로 ID
    private int outroID;           //아웃트로 ID

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
        Dictionary<string, object> stageTableInfo = CSVReader.FindRead("StageTable", "StageID", stageID);

        if (stageTableInfo != null)
        {
            try
            {
                stageWidth = (int)stageTableInfo["StageWidth"];
                stageHeight = (int)stageTableInfo["StageHeight"];
                playerSponeX = (int)stageTableInfo["PlayerSponeX"];
                playerSponeY = (int)stageTableInfo["PlayerSponeY"];
                monsterSponeID = (string)stageTableInfo["MonsterSponeID"];
                limitTime = (int)stageTableInfo["LimitTime"];
                limitAmount = (int)stageTableInfo["LimitAmount"];
                rewardID = (int[])stageTableInfo["RewardID"];
                rewardAmount = (int[])stageTableInfo["RewardAmount"];
                introID = (int)stageTableInfo["IntroID"];
                outroID = (int)stageTableInfo["OutroID"];

                return true;
            }
            catch (InvalidCastException invalidCastEx)
            {
                Debug.LogError("[LoadStageTable] Data Type Error");
            }
            catch (Exception e)
            {
                Debug.LogError("[LoadStageTable] Failed Load Table");

                //TODO :: 스테이지 입장 실패 처리 (로비 화면으로 이동)
            }
        }
        else
        {
            Debug.LogError("[LoadStageTable] stageTableInfo is null.");

            //TODO :: 스테이지 입장 실패 처리 (로비 화면으로 이동 or 파일 재검색)
        }

        return false;
    }
}
