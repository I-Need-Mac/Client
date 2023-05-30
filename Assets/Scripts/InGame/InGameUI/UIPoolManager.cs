using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPoolManager : SingletonBehaviour<UIPoolManager>
{
    private const string skillBtnPath = "Prefabs/UI/SkillButton";

    private ObjectPool<SkillUI> skillUIPool;

    protected override void Awake()
    {
        skillUIPool = new ObjectPool<SkillUI>(ResourcesManager.Load<SkillUI>(skillBtnPath), transform);
    }

    public SkillUI SpawnButton(Transform transform, Vector2 pos)
    {
        SkillUI skillUi = skillUIPool.GetObject();
        skillUi.transform.SetParent(transform);
        skillUi.transform.localPosition = pos;
        skillUi.transform.localScale = Vector3.one;
        //skillUi.SkillDataInit();
        skillUi.gameObject.SetActive(true);
        return skillUi;
    }

    public void DeSpawnButton(SkillUI skillUi)
    {
        skillUIPool.ReleaseObject(skillUi);
        skillUi.transform.SetParent(transform);
    }
}
