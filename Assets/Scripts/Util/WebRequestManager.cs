using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class WebRequestManager
{
    #region ResponseCode
    public enum EResponse
    {
        eSuccess = 200,
        eNotFount = 404,
        eQueryError = 500,
    }

    #endregion


    private bool isPostUsing = false;
    private Queue<int> postQueue = new Queue<int>();

    /// <summary>
    /// API 종료 여부
    /// </summary>
    private bool isAPIFinished = false;

    /// <summary>
    /// API 수신 성공 여부
    /// </summary>
    private bool isSuccessApiReceived = false;

    /// <summary>
    /// 타임아웃
    /// </summary>
    const float TIMEOUT = 3.0f;


    public const string WEBSERVICE_HOST = "http://ec2-3-34-48-14.ap-northeast-2.compute.amazonaws.com:8080";
    //Singleton을 활용하여 1개의 인스턴스 유지 및 접근 효율성 증가
    private static WebRequestManager _instance { get; set; }
    public static WebRequestManager Instance
    {
        get
        {
            return _instance ?? (_instance = new WebRequestManager());
        }
    }
    public bool IsConnectInternet()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DebugManager.Instance.PrintDebug("인터넷 끊힘");
            return false;
        }
        else return true;
    }
    /// <summary>
    /// Byte[] 형을 String 형으로 캐스팅 합니다.
    /// </summary>
    /// <param name="strByte"></param>
    /// <returns></returns>
    private string ByteToString(byte[] strByte)
    {
        return Encoding.Default.GetString(strByte);
    }

    /// <summary>
    /// Dictionary를 WWWForm으로 옮겨 WWWForm을 반환합니다.
    /// </summary>
    /// <param name="forms">[Dictionary : Map]</param>
    /// <returns></returns>
    private WWWForm GetWWWForm(Dictionary<string, string> forms)
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> value in forms)
        {
            form.AddField(value.Key, value.Value);
        }

        return form;
    }

    private string GetStringForm(Dictionary<string, string> forms)
    {
  
        string form = "";

        foreach (KeyValuePair<string, string> value in forms)
        {
            form += (value.Key + "=" + value.Value + "&");
        }

        return form = form.Substring(0, form.Length - 1);
        
    }






    public async Task<object> Get<T>(string url, Dictionary<string, string> data = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{WEBSERVICE_HOST}{url}?{GetStringForm(data)}"))
        {
            float timeout = 0f;
            request.SendWebRequest();


            Debug.Log($"{WEBSERVICE_HOST}{url}?{GetStringForm(data)}");
            while (!request.isDone)
            {
                timeout += Time.deltaTime;
                if (timeout > TIMEOUT)
                    return default;
                else
                    await Task.Yield();
            }
            Debug.Log(request.result);
            var jsonString = request.downloadHandler.text;
            var dataObj = JsonConvert.DeserializeObject<T>(jsonString);
            Debug.Log(dataObj);
            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {request.error}");

            return dataObj;

        }



        return default;

    }

    public async Task<object> Get<T>(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{WEBSERVICE_HOST}{url}"))
        {
            float timeout = 0f;
            request.SendWebRequest();


            Debug.Log($"{WEBSERVICE_HOST}{url}");
            while (!request.isDone)
            {
                timeout += Time.deltaTime;
                if (timeout > TIMEOUT)
                    return default;
                else
                    await Task.Yield();
            }
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dataObj = JsonConvert.DeserializeObject<T>(jsonString);
           // Debug.Log(dataObj);
            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {request.error}");
            Debug.Log(dataObj);
            return dataObj;

        }

        return default;

    }


    public async Task<object> Post<T>(string url, Dictionary<string, string> data)
    {
        using (UnityWebRequest request = UnityWebRequest.Post($"{WEBSERVICE_HOST}{url}", GetWWWForm(data)))
        {
            float timeout = 0f;
            request.SendWebRequest();
            while (!request.isDone)
            {
                timeout += Time.deltaTime;
                if (timeout > TIMEOUT)
                    return default;
                else
                    await Task.Yield();
            }
            var jsonString = request.downloadHandler.text;
            var dataObj = JsonConvert.DeserializeObject<T>(jsonString);
            Debug.Log(request.result);

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {request.error}");

            return dataObj;

        }



        return default;

    }


    public async Task<RECIEVE_LOGIN> RequestLogin(string ID)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("ID", "test data");
        return (RECIEVE_LOGIN)await Post<RECIEVE_LOGIN>(APIAdressManager.REQUEST_LOGIN, data);
    }

    public async Task<object> RequestGetTest()
    {
        return await Get<object>(APIAdressManager.REQUEST_GETTEST);
    }
}

public class RECIEVE_LOGIN
{
    public string ID;
}
