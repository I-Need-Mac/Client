using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxIcon : InGameUI
{
    RectTransform rect;
    private WaitForSeconds dimmedTime;

    private Image icon;
    private Image dimmed;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
        dimmed = transform.Find("Dimmed").GetComponent<Image>();
        dimmedTime = new WaitForSeconds(Time.fixedDeltaTime);
    }

    public void UiSetting(string path)
    {
        rect.sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
        Sprite sprite = ResourcesManager.Load<Sprite>(path);
        icon.sprite = sprite;
        dimmed.sprite = sprite;
        DimmedColorSetting(0, 0, 0, 150);
    }

    public void DimmedColorSetting(byte r, byte g, byte b, byte a)
    {
        dimmed.color = new Color32(r, g, b, a);
    }

    public void DimmedColorSetting(Color color)
    {
        dimmed.color = color;
    }

    public IEnumerator Dimmed(float time)
    {
        float coolTime = 0.0f;
        float amount = 1.0f;
        while (coolTime < time)
        {
            coolTime += Time.fixedDeltaTime;
            amount -= Time.fixedDeltaTime;
            dimmed.fillAmount = amount;
            yield return dimmedTime;
        }
    }
}
