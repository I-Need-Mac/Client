using System;
using System.Collections.Generic;
using UnityEngine;

public class UIData
{
    #region UI테이블 관련
    public enum UITable
    {
        StoryTable,
    }

    // 스토리 데이터
    static Dictionary<int, List<object>> storyTableData = new Dictionary<int, List<object>>();
    public static Dictionary<int, List<object>> StoryData { get { return storyTableData; } }

    static Dictionary<int, Dictionary<int, List<object>>> pageTableData = new Dictionary<int, Dictionary<int, List<object>>>();
    public static Dictionary<int, Dictionary<int, List<object>>> PageTableData { get { return pageTableData; } }

    public static void ReadData()
    {
        // 스토리 테이블을 읽습니다.
        storyTableData = CSVReader.FileRead("Table/" + Enum.GetName(typeof(UITable), UITable.StoryTable));

        int index = 0;
        foreach (KeyValuePair<int, List<object>> pair in storyTableData)
        {
            Debug.Log(pair.Key);

            // 테이블에 있는 페이지를 읽어옵니다.
            List<object> list = pair.Value;
            string pageTable = list[(int)UI_StoryBook.StoryTableInfo.StoryPath].ToString();
            Dictionary<int, List<object>> pageData = CSVReader.FileRead("Table/Story/" + pageTable);
            if (pageData == null)
                continue;

            if (pageData.Count == 0)
                continue;

            pageTableData.Add(pair.Key, pageData);
        }
    }
    #endregion
}
