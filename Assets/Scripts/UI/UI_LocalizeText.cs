using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LocalizeText : MonoBehaviour
{
    private TextMeshProUGUI textview;
    [SerializeField]
    public string localizeID;

    // Start is called before the first frame update
    void Start()
    {
        textview = GetComponent<TextMeshProUGUI>();
        textview.SetText(LocalizeManager.Instance.GetText(localizeID));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
