using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameSetter : MonoBehaviour
{
    // Start is called before the first frame update
    protected async void Start()  
    {
   
        SettingManager.Instance.ReadSettingFile(); //���� ���� ���� �ε�

        //WebRequestManager.Instance.ReceiveEventHandlerEvent += Instance_ReceiveEventHandlerEvent;
        //PostData_User();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("title", "test data");
        Debug.Log( await WebRequestManager.Instance.Get("",data));

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
