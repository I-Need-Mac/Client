using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TypeMeshProComponent : MonoBehaviour
{

    string textToType;
    TextMeshProUGUI textBox;
    public float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
     
     
    }
    public void SetTextToType(string input) {
        textToType = input;
    }

    public void TypeText(string textToType, Action onComplete = null)
    {
        this.textToType = textToType;
        // 이미 진행 중인 타이핑 코루틴이 있다면 중단
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // 새로운 타이핑 코루틴 시작
        typingCoroutine = StartCoroutine(TypeTextCoroutine(textToType, onComplete));
    }

    IEnumerator TypeTextCoroutine(string textToType, Action onComplete)
    {
        textBox.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        onComplete?.Invoke();
    }

    public void SkipTypeText()
    {
        // 타이핑 중단 및 전체 텍스트 즉시 표시
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            textBox.text = textToType; // 여기에서 textToType은 전체 텍스트를 저장하는 변수입니다.
            typingCoroutine = null;
        }
    }

    public bool IsSkippable()
    {
        // 타이핑 코루틴이 실행 중이면 건너뛸 수 있음
        return typingCoroutine != null;
    }

    public void ShowText() {
        textBox.text = textToType;
    }
}

