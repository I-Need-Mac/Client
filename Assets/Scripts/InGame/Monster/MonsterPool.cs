using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : ObjectPool<Monster>
{
    public void SetMonsterPrefab(string prefabPath)
    {
        prefab = ResourcesManager.Load<Monster>(prefabPath);
    }
}
