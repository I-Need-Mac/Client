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
        // csv read
        // 스토리 테이블 읽기
        //Dictionary<int, List<object>> storyData = CSVReader.FileRead("Table/StoryTable");
        //foreach (KeyValuePair<int, List<object>> pair in storyData)
        //{
        //    Debug.Log(pair.Key);

        //    // 테이블에 있는 페이지를 읽어옵니다.
        //    List<object> list = pair.Value;
        //    for( int i = 0; i < list.Count; i++ )
        //    {
        //    }
        //}

        //Dictionary<int, List<object>> pageData = CSVReader.FileRead("Table/Story/TEST1");

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