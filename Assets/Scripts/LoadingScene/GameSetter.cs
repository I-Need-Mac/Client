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

        SettingManager.Instance.ReadSettingFile();

        //WebRequestManager.Instance.ReceiveEventHandlerEvent += Instance_ReceiveEventHandlerEvent;
        //PostData_User();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("title", "test data");
       // RECIEVE_LOGIN t = await WebRequestManager.Instance.RequestLogin("");
       // t.ID = "";
       Debug.Log(await WebRequestManager.Instance.RequestGetTest());

    }

    // Update is called once per frame
    void Update()
    {

    }
}