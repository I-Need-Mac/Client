using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SorcererGauge : MonoBehaviour
{
    [SerializeField]
    string statNameLocaizeID;
    [SerializeField]
    TextMeshProUGUI statName;
    [SerializeField]
    TextMeshProUGUI gaugeValue;
    [SerializeField]
    Image gauge;
    
    private int nowValue;
    private int maxValue;

    // Start is called before the first frame update
    void Start()
    {
        statName.SetText(LocalizeManager.Instance.GetText(statNameLocaizeID));

    }

    public void SetValue(int nowValue, int maxValue = 0, string addString = "") {
        if (maxValue != 0) { 
            this.maxValue = maxValue;
        }
        this.nowValue = nowValue;

        float amount = (float)this.nowValue / (float)this.maxValue;
        gauge.fillAmount = amount;
        gaugeValue.SetText(this.nowValue +" / "+ this.maxValue + addString);


    }

    public void SetValue(double nowValue, double maxValue = 0,string addString="")
    {
        double amount = nowValue / maxValue;
        gauge.fillAmount = (float)amount;
        gaugeValue.SetText(nowValue + " / " + maxValue + addString);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
