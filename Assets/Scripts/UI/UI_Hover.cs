using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hover : MonoBehaviour
{
    [SerializeField]
    Sprite Origin;
    [SerializeField]
    Sprite Hover;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void OnEnterHover(PointerEventData data)
    //{
    //    if (loginButtonHover != null)
    //    {
    //        GetImage((int)Images.LoginButton).gameObject.GetComponent<Image>().sprite = loginButtonHover;
    //    }
    //}

    //public void OnExitHover(PointerEventData data)
    //{
    //    if (loginButtonOrigin != null)
    //    {
    //        GetImage((int)Images.LoginButton).gameObject.GetComponent<Image>().sprite = loginButtonOrigin;
    //    }
    //}
}
