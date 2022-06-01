using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager
{
    public delegate void ReceiveEventHandler(object obj);
    public event ReceiveEventHandler ReceiveEventHandlerEvent;
    public string WEBSERVICE_HOST = "http://ec2-3-34-48-14.ap-northeast-2.compute.amazonaws.com:8080/";

    //Singleton을 활용하여 1개의 인스턴스 유지 및 접근 효율성 증가
    private static WebRequestManager _instance { get; set; }
    public static WebRequestManager Instance
    {
        get
        {
            return _instance ?? (_instance = new WebRequestManager());
        }
    }

    //Get 함수 제네릭 타입을 활용하여 다양한 타입 리턴값 대응
    public IEnumerator Get<T>(string uri, string prameter = "")
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{WEBSERVICE_HOST}/{uri}/{prameter}"))
        {
            yield return request.SendWebRequest();
            try
            {
                var jsonString = request.downloadHandler.text;

                //json 객체로 변환
                var dataObj = JsonConvert.DeserializeObject<T>(jsonString);

                //이벤트로 데이터를 보낸다.이벤트 등록한곳에서 받을 수 있게
                ReceiveEventHandlerEvent(dataObj);
            }
            catch (Exception e)
            {

            }
        }
    }

    public IEnumerator Get<T>(string uri, WWWForm parameter)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{WEBSERVICE_HOST}/{uri}/{parameter.data}"))
        {
            yield return request.SendWebRequest();
            try
            {
                var jsonString = request.downloadHandler.text;

                //json 객체로 변환
                var dataObj = JsonConvert.DeserializeObject<T>(jsonString);

                //이벤트로 데이터를 보낸다.이벤트 등록한곳에서 받을 수 있게
                ReceiveEventHandlerEvent(dataObj);
            }
            catch (Exception e)
            {

            }
        }
    }

    public IEnumerator Post(string uri, WWWForm parameter)
    {
        using (UnityWebRequest request = UnityWebRequest.Post($"{WEBSERVICE_HOST}/{uri}", parameter))
        {
            yield return request.SendWebRequest();
            Debug.Log(request.result);
        }
    }

    //Post 함수 Json객체를 활용하여 파라미터를 내보낸다
    //핵심은 json 파싱하여 객체로 보내면 json 형식이 깨져버려서 반드시 byte로 변환 후 보내야한다.
    //   request.uploadHandler와  request.SetRequestHeader이 핵심이다
    public IEnumerator Post(string uri, string parameter)
    {
        using (UnityWebRequest request = UnityWebRequest.Post($"{WEBSERVICE_HOST}/{uri}", parameter))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(parameter);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log(request.result);
        }
    }


    public WWWForm Parameter_point(string point, string userCode, string itemCode)
    {
        WWWForm form = new WWWForm();
        form.AddField("userCode", userCode);
        form.AddField("point", point);
        form.AddField("itemCode", itemCode);
        return form;
    }
}
