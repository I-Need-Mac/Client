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

    //Singleton�� Ȱ���Ͽ� 1���� �ν��Ͻ� ���� �� ���� ȿ���� ����
    private static WebRequestManager _instance { get; set; }
    public static WebRequestManager Instance
    {
        get
        {
            return _instance ?? (_instance = new WebRequestManager());
        }
    }

    //Get �Լ� ���׸� Ÿ���� Ȱ���Ͽ� �پ��� Ÿ�� ���ϰ� ����
    public IEnumerator Get<T>(string uri, string prameter = "")
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{WEBSERVICE_HOST}/{uri}/{prameter}"))
        {
            yield return request.SendWebRequest();
            try
            {
                var jsonString = request.downloadHandler.text;

                //json ��ü�� ��ȯ
                var dataObj = JsonConvert.DeserializeObject<T>(jsonString);

                //�̺�Ʈ�� �����͸� ������.�̺�Ʈ ����Ѱ����� ���� �� �ְ�
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

                //json ��ü�� ��ȯ
                var dataObj = JsonConvert.DeserializeObject<T>(jsonString);

                //�̺�Ʈ�� �����͸� ������.�̺�Ʈ ����Ѱ����� ���� �� �ְ�
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

    //Post �Լ� Json��ü�� Ȱ���Ͽ� �Ķ���͸� ��������
    //�ٽ��� json �Ľ��Ͽ� ��ü�� ������ json ������ ���������� �ݵ�� byte�� ��ȯ �� �������Ѵ�.
    //   request.uploadHandler��  request.SetRequestHeader�� �ٽ��̴�
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
