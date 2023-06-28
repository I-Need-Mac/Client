using System;
using System.Collections.Generic;
using UnityEngine;

public class UIData
{
    #region UI테이블 관련
    public enum UITable
    {
        StoryTable,
        StageTable,
        ChapterTable,
        CharacterTable,
        SkillTable
    }

    public enum CharacterTableCol
    {
        CharacterId,
        CharacterName,          // 캐릭터 이름
        MainShowImagePath,    // 캐릭터 선택 이미지 경로
        SkillID_01,              // 캐릭터 궁극기 id
        SkillID_02              // 캐릭터 스킬 id
    }

    public enum SkillTableCol
    {
        Name,
        Desc,
        SkillImage,
    }

    public enum StageTableCol
    {
        StageID,
        IntroID,
        OutroID,
        ChapterCategory,
    }

    public enum ChapterTableCol
    {
        ChapterID,
        ChapterHeader,
        ChapterName,
        ChapterImage,
    }

    // 스토리 데이터
    static Dictionary<string, Dictionary<string, object>> storyTableData = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, Dictionary<string, object>> StoryData { get { return storyTableData; } }
    // 스토리북용 데이터 셋팅 데이터    
    static Dictionary<int, Dictionary<int, List<object>>> pageTableData = new Dictionary<int, Dictionary<int, List<object>>>();
    public static Dictionary<int, Dictionary<int, List<object>>> PageTableData { get { return pageTableData; } }

    // 스테이지 데이터
    static Dictionary<string, Dictionary<string, object>> stageTableData = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, Dictionary<string, object>> StageData { get { return stageTableData; } }

    // 챕터 데이터
    static Dictionary<string, Dictionary<string, object>> chapterTableData = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, Dictionary<string, object>> ChapterData { get { return chapterTableData; } }

    // 캐릭터 데이터
    static Dictionary<string, Dictionary<string, object>> characterTableData = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, Dictionary<string, object>> CharacterData { get { return characterTableData; } }

    // 스킬 데이터
    static Dictionary<string, Dictionary<string, object>> skillTableData = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, Dictionary<string, object>> SkillData { get { return skillTableData; } }


    public static void ReadData()
    {
        ReadStoryData();
        ReadChapterData();
        ReadStageData();
        ReadCharacterData();
        ReadSkillData();
    }

    static void ReadStoryData()
    {
        // 스토리 테이블을 읽습니다.
        storyTableData = CSVReader.Read(Enum.GetName(typeof(UITable), UITable.StoryTable));

        foreach (KeyValuePair<string, Dictionary<string, object>> pair in storyTableData)
        {
            if (pair.Key == "")
                continue;

            Debug.Log(pair.Key);

            // 테이블에 있는 페이지를 읽어옵니다.
            Dictionary<string, object> list = pair.Value;

            object pageTable;
            list.TryGetValue(UI_StoryBook.StoryTableInfo.StoryPath.ToString(), out pageTable);
            //string pageTable = list[(int)UI_StoryBook.StoryTableInfo.StoryPath].ToString();
            Dictionary<string, Dictionary<string, object>> pageData = CSVReader.Read("Story/" + pageTable);
            if (pageData == null)
                continue;

            if (pageData.Count == 0)
                continue;

            Dictionary<int, List<object>> addList = new Dictionary<int, List<object>>();
            foreach (KeyValuePair<string, Dictionary<string, object>> pagePair in pageData)
            {
                if (pagePair.Key == "")
                    continue;

                List<object> createList = new List<object>();
                foreach (KeyValuePair<string, object> valuePair in pagePair.Value)
                {
                    createList.Add(valuePair.Value);
                }

                addList.Add(int.Parse(pagePair.Key), createList);
            }

            // 스토리북으로 쓸 셋팅데이터 add
            pageTableData.Add(int.Parse(pair.Key), addList);
        }
    }

    static void ReadStageData()
    {
        // 스테이지 테이블을 읽습니다.
        stageTableData = CSVReader.Read(Enum.GetName(typeof(UITable), UITable.StageTable));

        // 스테이지 id를 임의로 넣어둡니다 (추후 초기 셋팅은 게임 시작 시 서버에서 가져온 스테이지 값을 넣도록 합니다)
        UIManager.Instance.selectStageID = 10102;
    }

    static void ReadChapterData()
    {
        // 챕터 테이블을 읽습니다.
        chapterTableData = CSVReader.Read(Enum.GetName(typeof(UITable), UITable.ChapterTable));
    }

    static void ReadCharacterData()
    {
        // 캐릭터 테이블을 읽습니다.
        characterTableData = CSVReader.Read(Enum.GetName(typeof(UITable), UITable.CharacterTable));
        foreach (string val in characterTableData.Keys)
        {
            UIManager.Instance.selectCharacterID = int.Parse(val);
            break;
        }
    }

    static void ReadSkillData()
    {
        // 스킬 테이블을 읽습니다.
        skillTableData = CSVReader.Read(Enum.GetName(typeof(UITable), UITable.SkillTable));
    }
    #endregion
}
