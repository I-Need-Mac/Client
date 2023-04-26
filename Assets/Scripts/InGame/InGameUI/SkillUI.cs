using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> skillTable;

    private LevelUpUI levelUpUi;

    private TextMeshProUGUI skillName;
    private TextMeshProUGUI skillInfo;

    public Button btn { get; private set; }
    public int skillId { get; private set; }

    private void Awake()
    {
        skillTable = CSVReader.Read("SkillTable");
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CloseBox);

        skillName = transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
        skillInfo = transform.Find("SkillInfo").GetComponent<TextMeshProUGUI>();
    }

    private void CloseBox()
    {
        levelUpUi = transform.GetComponentInParent<LevelUpUI>();
        GameManager.Instance.player.playerManager.playerData.skills.Add(skillId, new SkillInfo(null, null));
        Time.timeScale = 1f;

        foreach (SkillUI ui in levelUpUi.skillUis)
        {
            UIPoolManager.Instance.DeSpawnButton(ui);
        }

        levelUpUi.gameObject.SetActive(false);
    }

    public void SkillDataInit()
    {
        levelUpUi = transform.GetComponentInParent<LevelUpUI>();
        do
        {
            if (levelUpUi.skillCount < 8)   //아직 스킬 8개가 다 안채워졌을때
            {
                bool flag = true;
                do
                {
                    skillId = UnityEngine.Random.Range(101, 200) * 100 + 1;
                    if (!skillTable.ContainsKey(skillId.ToString()))
                    {
                        continue;
                    }

                    flag = false;
                    foreach (int id in GameManager.Instance.player.playerManager.playerData.skills.Keys)
                    {
                        if (id / 100 == skillId / 100)    //가지고 있는 스킬일 때
                        {
                            if (id % 100 != 99)    //만렙이 아니라면
                            {
                                skillId = id + 1;
                            }
                            else
                            {
                                flag = true;
                            }
                            break;
                        }
                    }
                } while (flag);

                //if (!skillTable.ContainsKey(skillId.ToString()))
                //{
                //    skillId = 99999;
                //}
            }
            else
            {
                int count = 0;
                do
                {
                    if (count == 8) //이미 가지고있는 모든 스킬이 만렙일때
                    {
                        gameObject.SetActive(false);
                        break;
                    }
                    skillId = GameManager.Instance.player.playerManager.playerData.skills.Keys.ElementAt(UnityEngine.Random.Range(0, 8));
                    ++count;
                } while (skillId % 100 == 99);
                ++skillId;
            }
        } while (levelUpUi.skills.Contains(skillId));
        levelUpUi.skills.Add(skillId);

        Dictionary<string, object> info = skillTable[skillId.ToString()];
        skillName.text = info["Name"].ToString();
        skillInfo.text = info["Desc"].ToString();
    }
}
