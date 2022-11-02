using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour
{
    public InputField InputSceneNum;
    private string SceneNum;
    private Dictionary<string, Dictionary<string, object>> SceneData = new Dictionary<string, Dictionary<string, object>>();

    public CanvasGroup can_Main;
    public CanvasGroup can_Map1;
    public CanvasGroup can_Map2;
    public CanvasGroup can_Map3;

    private MapName currentname;

    private void Start()
    {
        SceneData = CSVReader.IDRead("StageTable");
    }

    public void OnBtnClick()
    {
        SceneNum = InputSceneNum.text;
        Debug.Log(SceneNum);
        string c_MapName = Convert.ToString(SceneData[SceneNum]["MapID"]);
        Debug.Log(c_MapName);
        currentname = (MapName)Enum.Parse(typeof(MapName), c_MapName);
        
        switch(currentname)
        {
            case MapName.Map1:
                CanvasGroupOff(can_Main);
                CanvasGroupOn(can_Map1);
                break;
            case MapName.Map2:
                CanvasGroupOff(can_Main);
                CanvasGroupOn(can_Map2);
                break;
            case MapName.Map3:
                CanvasGroupOff(can_Main);
                CanvasGroupOn(can_Map3);
                break;
        }
        
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
