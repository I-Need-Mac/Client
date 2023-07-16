using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAddButton : MonoBehaviour
{
    private TMP_InputField inputField;
    private Button button;

    private void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        button = GetComponentInChildren<Button>();

        button.onClick.AddListener(test);
    }


    private void test()
    {
        int skillId = int.Parse(inputField.text);
        SkillManager.Instance.SkillAdd(skillId, GameManager.Instance.player.transform, PlayerUI.Instance.skillCount);
    }
}
