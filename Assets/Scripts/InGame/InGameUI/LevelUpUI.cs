using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    private const int SKILL_MAX_LEVEL = 8;

    private Dictionary<string, Dictionary<string, object>> skillTable;
    private Transform body;
    private RectTransform bodyRect;

    public List<SkillUI> skillUis { get; private set; } = new List<SkillUI>();
    public int skillCount { get; private set; } = 0;
    public List<int> skills { get; private set; } = new List<int>();   //가진 스킬이 아니라 ui에 올라온 스킬 목록 (중복 방지)

    private void Awake()
    {
        skillTable = CSVReader.Read("SkillTable");
        body = transform.Find("Body");
        bodyRect = body.GetComponent<RectTransform>();
    }

    private void CloseBox(int skillId)
    {
        //GameManager.Instance.player.playerManager.playerData.skills.Add(skillId, new SkillInfo(null, null));

        foreach (SkillUI ui in skillUis)
        {
            UIPoolManager.Instance.DeSpawnButton(ui);
        }

        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void SkillBoxInit(int num)
    {
        float height = 175 + 120 * num;
        bodyRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        for (int i = 0; i < num; i++)
        {
            Vector2 pos = new Vector2(0, height * 0.5f - 175 - 120 * i);
            SkillUI skillUi = UIPoolManager.Instance.SpawnButton(body.transform, pos);
            int skillId = RandomSkillId();
            skillUi.SkillBtnInit(skillTable[skillId.ToString()]);
            skillUi.btn.onClick.RemoveAllListeners();
            skillUi.btn.onClick.AddListener(() => CloseBox(skillId));
            skillUis.Add(skillUi);
        }
    }

    private int RandomSkillId()
    {
        //Dictionary<int, SkillInfo> skillData = GameManager.Instance.player.playerManager.playerData.skills;
        //int skillId = 0;

        //if (skillCount < 8) //스킬칸이 남은 경우
        //{
        //    while (true)
        //    {
        //        skillId = UnityEngine.Random.Range(101, 200);
        //        if (skills.Contains(skillId))
        //        {
        //            continue;
        //        }
        //        skillId = skillId * 100 + 1;
        //        if (!skillTable.ContainsKey(skillId.ToString()))    //스킬 존재여부
        //        {
        //            continue;
        //        }

        //        int count = skillData.Count;
                
        //        if (count == 0)
        //        {
        //            return skillId;
        //        }

        //        foreach (int id in skillData.Keys)
        //        {
        //            if (id / 100 == skillId / 100) //가지고 있는 스킬일 때
        //            {
        //                if (id % 100 != SKILL_MAX_LEVEL) //만렙이 아니라면
        //                {
        //                    skillId = id + 1;
        //                    skills.Add(skillId / 100);
        //                    return skillId;
        //                }
        //            }
        //            if (--count == 0) //새로운 스킬일 때
        //            {
        //                return skillId;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    int index = UnityEngine.Random.Range(0, 8);
        //    for (int i = 0; i < 8; i++)
        //    {
        //        skillId = skillData.Keys.ElementAt((i + index) % 8);
        //        if (skillId % 100 != SKILL_MAX_LEVEL)
        //        {
        //            skills.Add(skillId / 100);
        //            return skillId;
        //        }
        //    }
        //}

        return 99999;
    }
}
