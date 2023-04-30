using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private Image skillIcon;
    private TextMeshProUGUI skillName;
    private TextMeshProUGUI skillInfo;

    public Button btn { get; private set; }
    public int skillId { get; private set; }

    private void Awake()
    {
        btn = GetComponent<Button>();

        skillIcon = transform.Find("Icon").GetComponent<Image>();
        skillName = transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
        skillInfo = transform.Find("SkillInfo").GetComponent<TextMeshProUGUI>();
    }

    public void SkillBtnInit(Dictionary<string, object> skillData)
    {
        skillIcon.sprite = ResourcesManager.Load<Sprite>(skillData["SkillImage"].ToString());
        skillName.text = skillData["Name"].ToString();
        skillInfo.text = skillData["Skill_TestExplain1"].ToString();
    }

}
