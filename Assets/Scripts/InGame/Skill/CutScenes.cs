using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScenes : MonoBehaviour
{
    [SerializeField] private float slowStartPosX = 550.0f;
    [SerializeField] private float disAppearSpeed = 2.5f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float blinkSpeed = 0.01f;

    private RectTransform rect;
    private Rigidbody2D rigid;
    private Image cutImage;

    private bool slow;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rigid = GetComponent<Rigidbody2D>();
        cutImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (rect.anchoredPosition.x >= slowStartPosX && slow)
        {
            slow = false;
            StartCoroutine(Blink());
        }

        if (slow)
        {
            transform.Translate(Vector2.right * moveSpeed * 10000.0f * Time.unscaledDeltaTime);
            moveSpeed *= 0.99f;
        }
        
        //DebugManager.Instance.PrintError("Velocity: {0}", rigid.velocity);
        //if (rect.anchoredPosition.x >= slowStartPosX && slow)
        //{
        //    rigid.velocity *= -0.1f;
        //    slow = false;
        //    StartCoroutine(Blink());
        //}


        //if (rect.anchoredPosition.x >= disAppearPosX)
        //{
        //    cutImage.fillAmount -= Time.deltaTime * disAppearSpeed;
        //}

        //if (cutImage.fillAmount <= 0.0f)
        //{
        //    SceneManager.UnloadSceneAsync("CutScenes");
        //}
    }

    private void Start()
    {
        rect.anchoredPosition = new Vector2(-Screen.width, rect.anchoredPosition.y);
        slow = true;
        Time.timeScale = 0.0f;
    }

    private IEnumerator Blink()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        WaitForSecondsRealtime blinkTime = new WaitForSecondsRealtime(blinkSpeed);
        float time = disAppearSpeed;
        
        for (int i = 0; i < 3; i++)
        {
            cutImage.fillAmount = 0.0f;
            yield return blinkTime;
            cutImage.fillAmount = 1.0f;
            yield return new WaitForSecondsRealtime(time);
            time *= 0.25f;
        }

        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }

}
