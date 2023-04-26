using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    private Transform body;
    private RectTransform bodyRect;

    public List<SkillUI> skillUis { get; private set; } = new List<SkillUI>();
    public int skillCount { get; private set; } = 0;
    public List<int> skills { get; private set; } = new List<int>();   //가진 스킬이 아니라 ui에 올라온 스킬 목록 (중복 방지)

    private void Awake()
    {
        body = transform.Find("Body");
        bodyRect = body.GetComponent<RectTransform>();
    }

    public void SkillBoxInit(int num)
    {
        float height = 175 + 120 * num;
        bodyRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        for (int i = 0; i < num; i++)
        {
            Vector2 pos = new Vector2(0, height * 0.5f - 175 - 120 * i);
            skillUis.Add(UIPoolManager.Instance.SpawnButton(body.transform, pos));
        }
    }
}


//private Dictionary<string, Dictionary<string, object>> skillTable;

//private Button[] selectBtns;
//private Text[] skillInfoText;

//private void Awake()
//{
//    Init();
//}

//private void Init()
//{
//    skillTable = CSVReader.Read("SkillTable");

//    selectBtns = GetComponentsInChildren<Button>();
//    skillInfoText = GetComponentsInChildren<Text>().Where(text => !text.text.Equals("Refresh")).ToArray();

//    foreach (Button btn in selectBtns)
//    {
//        if (btn.name.Contains("SkillButton"))
//        {
//            btn.onClick.AddListener(SkillSelectWindowClose);
//        }
//        else
//        {
//            btn.onClick.AddListener(RefreshSkillWindow);
//        }

//        btn.gameObject.SetActive(false);
//    }
//    SkillInfoAssign();
//}

//private void SkillInfoAssign()
//{
//    foreach (Text info in skillInfoText)
//    {
//        info.text = skillTable.Keys.ElementAt(UnityEngine.Random.Range(0, skillTable.Count - 1));
//    }
//}

//public void SkillSelectWindowOpen()
//{
//    Time.timeScale = 0f;
//    foreach (Button btn in selectBtns)
//    {
//        btn.gameObject.SetActive(true);
//    }
//}

//private void SkillSelectWindowClose()
//{
//    Text selectedBtn = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
//    DebugManager.Instance.PrintDebug("##: " + selectedBtn.name);
//    GameManager.Instance.player.Fire(Convert.ToInt32(selectedBtn.text));
//    foreach (Button btn in selectBtns)
//    {
//        btn.gameObject.SetActive(false);
//    }
//    Time.timeScale = 1.0f;
//}

//private void RefreshSkillWindow()
//{
//    SkillInfoAssign();
//}
