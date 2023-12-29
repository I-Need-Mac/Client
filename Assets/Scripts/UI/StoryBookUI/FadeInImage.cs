using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 2.0f;
    private Coroutine fadeCoroutine;
    private bool isFading = false;
    private int fadingType = 0; //-1 아웃, 1 인, 0 없음

    private void Start()
    {
      SetFadeOut();
    }


    // 페이드 인 시작
    public void StartFadeIn(Action onComplete = null)
    {
        fadingType=1;
        StartFade(0, 1, onComplete);

    }

    // 페이드 아웃 시작
    public void StartFadeOut(Action onComplete = null)
    {
        fadingType = -1;
        StartFade(1, 0, onComplete);
    }

    private void StartFade(float startAlpha, float endAlpha, Action onComplete)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Fade(startAlpha, endAlpha, onComplete));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, Action onComplete)
    {
        isFading = true;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            imageToFade.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        isFading = false;
        fadingType = 0;
        onComplete?.Invoke();
    }

    public bool IsSkippable()
    {
        return isFading;
    }

    // 페이드 스킵 (페이드 인 또는 페이드 아웃)
    public void SkipFade(Action onComplete = null)
    {
        if (isFading && fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            imageToFade.color = new Color(1, 1, 1, fadingType >= 0 ? 1 : 0); // 투명도를 반대 상태로 즉시 설정
            isFading = false;
            onComplete?.Invoke();
        }
    }

    public bool isFadeIn() { 
        return  fadingType == 1 ;
    }
    public bool isFadeOut()
    {
        return fadingType == -1;
    }
    public bool isFadeNone()
    {
        return fadingType == 0;
    }

    public void SetFadeOut() {
        imageToFade.color = new Color(1, 1, 1, 0);
    }

    public void SetFadeIn() {
        imageToFade.color = new Color(1, 1, 1, 1);
    }
}
