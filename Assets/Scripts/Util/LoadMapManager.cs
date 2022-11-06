using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapManager : SingleTon<LoadMapManager>
{
    private Dictionary<string, Dictionary<string, object>> sceneData = new Dictionary<string, Dictionary<string, object>>();

    public LoadMapManager()
    {
        sceneData = CSVReader.Read("StageTable");
    }

    public string SceneNumberToMapName(string sceneNumber)
    {
        DebugManager.Instance.PrintDebug("[LoadMapManager] Loading Scene Number : " + sceneNumber);

        string returnMapName;
        returnMapName = Convert.ToString(sceneData[sceneNumber]["MapID"]);
        DebugManager.Instance.PrintDebug("[LoadMapManager] Loading Map Name : " + returnMapName);

        return returnMapName;
    }

    public GameObject LoadMapNameToMapObject(string mapName)
    {
        GameObject returnMap;
        returnMap = PrefabLoader.Instance.LoadPrefabToGameObject(mapName);
        DebugManager.Instance.PrintDebug("[LoadMapManager] Loading Map : " + mapName);
        return returnMap;
    }
}