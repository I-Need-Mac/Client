using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : SingleTon<APIManager>
{
    Crypto crypto;
    WebRequestManager requestManager;
    public APIManager()
    {
        crypto = new Crypto();
        requestManager = new WebRequestManager(crypto);
    }

    public void SetPublicKeyPEM(TextAsset pem)
    {
        crypto.SetPublicKeyPEM(pem);

    }
    public void SetPrivateKeyPEM(TextAsset pem)
    {
        crypto.SetPrivateKeyPEM(pem);

    }

    public async Task<bool> CheckNicknameDuplicated(string data)
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("name", data);
       DuplicatedNickName duplicatedNickName=  (DuplicatedNickName)await requestManager.Get<DuplicatedNickName>(APIAddressManager.REQUEST_CHECKNAME, sendData);
        return duplicatedNickName.data.isDuplicated;
    }
    public async Task<NormalResult> TryRegist(string name, string nickname)
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", name);
        sendData.Add("name", nickname);
        return (NormalResult)await requestManager.Post<NormalResult>(APIAddressManager.REQUEST_REGIST, sendData);
    }
    public async Task<bool> TryLogin(string name)
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", name);
        NormalResult result = (NormalResult)await requestManager.Post<NormalResult>(APIAddressManager.REQUEST_LOGIN, sendData);

        DebugManager.Instance.PrintDebug("[WebRequest] "+"Login Result for "+name+" result : "+result.statusCode);
        if (result.statusCode == 200) { 
            return true;
        }
        return false;

    }
    public async Task<bool> CheckCharacterUnlock(string name, string nickname,string characterName)
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", name);
        sendData.Add("name", nickname);
        StartGame startGame = (StartGame)await requestManager.Get<StartGame>(APIAddressManager.REQUEST_GAME_START, sendData);
        //UIStatus.Instance.Hojin = startGame.data.hojin;
        //UIStatus.Instance.Seimei = startGame.data.seimei;
        //UIStatus.Instance.Macia = startGame.data.macia;
        //UIStatus.Instance.SiWoo = startGame.data.siWoo;
        //UIStatus.Instance.Sinwol = startGame.data.sinwol;
        //UIStatus.Instance.Ulises = startGame.data.ulises;
        switch (characterName.ToLower())
        {
            case "hojin":
                return startGame.data.hojin;
            case "seimei":
                return startGame.data.seimei;
            case "macia":
                return startGame.data.macia;
            case "siwoo":
                return startGame.data.siWoo;
            case "sinwol":
                return startGame.data.sinwol;
            case "ulises":
                return startGame.data.ulises;
            default:
                Debug.LogError("There's no such character");
                return false;
        }
    }
    public async Task<int> StageLastClear(string name, string nickname)
    {
        Dictionary<string, string> sendData = new Dictionary<string, string>();
        sendData.Add("steam_id", name);
        sendData.Add("name", nickname);
        StartGame startGame = (StartGame)await requestManager.Get<StartGame>(APIAddressManager.REQUEST_GAME_START, sendData);
        UIStatus.Instance.Last_Clear_Stage = startGame.data.last_stage.GetValueOrDefault();
        return startGame.data.last_stage.GetValueOrDefault();
    }


}
