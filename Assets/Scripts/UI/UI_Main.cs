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

    }

    async void WebConnectFromGet()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("name", "AA");

        //DuplicatedNickName duplicatedNickName = await APIManager.Instance.CheckNicknameDuplicated<DuplicatedNickName>(sendData);
       // Debug.Log(duplicatedNickName.data.isDuplicated);
    }


    async void WebHandShakeFromPost()
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", "12213");
        sendData.Add("name", "dass2s");
    

       var data = await APIManager.Instance.TryRegist(sendData);

    }

    private void Update()
    {
       
    }
}