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
    static Dictionary<string, Dictionary<string, object>> storyTableData = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, Dictionary<string, object>> StoryData { get { return storyTableData; } }

    static Dictionary<int, Dictionary<int, List<object>>> pageTableData = new Dictionary<int, Dictionary<int, List<object>>>();
    public static Dictionary<int, Dictionary<int, List<object>>> PageTableData { get { return pageTableData; } }

    public static void ReadData()
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
                foreach(KeyValuePair<string, object> valuePair in pagePair.Value)
                {
                    createList.Add(valuePair.Value);
                }

                addList.Add(int.Parse(pagePair.Key), createList);
            }

            pageTableData.Add(int.Parse(pair.Key), addList);
        }
    }
    #endregion
}
