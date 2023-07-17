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
        //WebConnectFromGet();
        //WebConnectFromPost();
        //WebLoginFromPost();
        //WebHandShakeFromPost();
    }

    async void WebConnectFromGet()
    {
        var data = await WebRequestManager.Instance.Get<Dictionary<string, string>>("user/user_list");
    }

    async void WebConnectFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "test_steam_id");
        sendData.Add("nick_name", "test_nick_name");
        sendData.Add("admin_level", "0");

        var data = await WebRequestManager.Instance.Post<Dictionary<string, string>>("/user/make_user", sendData);
    }

    async void WebLoginFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "mongplee92");

        var data = await WebRequestManager.Instance.Post<Dictionary<string, string>>("user/login", sendData);
    }

    async void WebHandShakeFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "mongplee92");
        sendData.Add("nick_name", "mongplee92");
        sendData.Add("admin_level", "0");

        var data = await WebRequestManager.Instance.Post<Dictionary<string, string>>("user/handshake", sendData);
    }

    private void Update()
    {
       
    }
}