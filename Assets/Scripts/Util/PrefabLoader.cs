using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrefabLoader : SingleTon<PrefabLoader>
{
    private Dictionary<string, GameObject> cachedPrefab = new Dictionary<string, GameObject>();

    const string MAP_PREFAB_HOME = "MapPrefabs/";

    public PrefabLoader()
    {
        cachedPrefab.Add("Dummy", Resources.Load(MAP_PREFAB_HOME + "Dummy", typeof(GameObject)) as GameObject);
    }


    public GameObject LoadPrefabToGameObject(string path)
    {
        GameObject returnPrefab;
        if (cachedPrefab.ContainsKey(path))
        {
            DebugManager.Instance.PrintDebug("[PrefabLoader] Cached Prefab : " + path);
            returnPrefab = cachedPrefab[path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[PrefabLoader] New Prefab : " + path);

            returnPrefab = Resources.Load(MAP_PREFAB_HOME + path, typeof(GameObject)) as GameObject;
            if (returnPrefab == null)
            {
                DebugManager.Instance.PrintDebug("[PrefabLoader] Dummy Prefab : 경로 이상");
                returnPrefab = cachedPrefab["Dummy"];
            }
            else { cachedPrefab.Add(path, returnPrefab); }

        }
        return returnPrefab;
    }

}
