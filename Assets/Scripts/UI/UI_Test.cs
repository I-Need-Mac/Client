using System.Collections.Generic;
using UnityEngine;

// UI테스트용
public class UI_Test : MonoBehaviour
{
    private void Awake()
    {
        // manager init
        UIManager.Instance.Init();

        // csv read
        //Dictionary<int, List<object>> csvRead = CSVReader.FileRead("Table/StoryTable");
    }

    void Start()
    {
        //UIManager.Instance.ShowUI(UIManager.UI_Prefab.UI_StoryBook);
        // csv read
        //CSVReader.FileRead("Table/StoryTable");
        //CSVReader.FileRead("Table/PageTable");

        //int cnt = 0;
        //foreach(KeyValuePair<int, List<object>> pair in UIManager.Instance.page)
        //{
        //    UIManager.Instance.SetBookPage<UI_Book>();
        //    UIManager.Instance.bookPageAry[cnt].page = pair.Key;
        //    cnt++;
        //}

        //UIManager.Instance.bookPageObj[0].SetActive(true);
    }
}