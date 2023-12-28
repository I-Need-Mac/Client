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


}
