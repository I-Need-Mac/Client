using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// UI테스트용
public class UI_Main : MonoBehaviour
{
    private void Awake()
    { 
        SoundManager.Instance.CreateSoundManager();

        // manager init
        UIManager.Instance.Init();
    }

    void Start()
    {

        WebConnectFromGet();
        WebHandShakeFromPost();
    }

    async void WebConnectFromGet()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("name", "AA");
        DuplicatedNickName data = (DuplicatedNickName)await WebRequestManager.Instance.Get<DuplicatedNickName>(APIAdressManager.REQUEST_CHECKNAME, sendData);
        Debug.Log(data.data.isDuplicated);
    }


    async void WebHandShakeFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "1213");
        sendData.Add("name", "dasss");
    

        var data = await WebRequestManager.Instance.Post<Dictionary<string, string>>(APIAdressManager.REQUEST_REGIST, sendData);

    }

    private void Update()
    {
       
    }
}