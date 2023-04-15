using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private Button reStartBtn;

    private void Awake()
    {
        reStartBtn = GetComponentInChildren<Button>();
        reStartBtn.onClick.AddListener(ReStart);
    }

    private void ReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }
}
