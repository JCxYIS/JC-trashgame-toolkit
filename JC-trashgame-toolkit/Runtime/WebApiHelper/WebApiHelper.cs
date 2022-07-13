using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.AddressableAssets;

public class WebApiHelper : MonoSingleton<WebApiHelper>
{
    public static string ROOT_URL = "";
    public static string _debugCookie = "";

    // WebGL jslibs
    // [DllImport("__Internal")]
    // private static extern string GetRootUrl(); // 注意 CORS



    protected override void Init()
    {

    }

    private void Update()
    {

    }

    /* -------------------------------------------------------------------------- */

    /// <summary>
    /// 呼叫 API
    /// callback: isSuccess, data
    /// </summary>
    public void CallApi<T>(string method, string route, Action<bool, T> callback)
    {
        CallApi<T>(method, route, null, (succ, msg, data)=>callback?.Invoke(succ, data));
    }

    public void CallApi<T>(string method, string route, Action<bool, string, T> callback)
    {
        CallApi<T>(method, route, null, callback);
    }
    
    /// <summary>
    /// 呼叫 API 配上 payload
    /// </summary>
    public void CallApi<T>(string method, string route, object payload, Action<bool, T> callback)
    {
        CallApi<T>(method, route, payload, (succ, msg, data)=>callback?.Invoke(succ, data));
    }

    public void CallApi<T>(string method, string route, object payload, Action<bool, string, T> callback)
    {
        method = method.ToUpper();
        UnityWebRequest www = new UnityWebRequest(Path.Combine(ROOT_URL, route), method);

        // attach payload
        if(payload != null)
        {
            string payloadStr = JsonUtility.ToJson(payload);
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(payloadStr);
            // Debug.Log($"[WebAPI] Request Payload size={postBytes}\nContent={payloadStr}");
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        }

        // start www
        Debug.Log($"[WebAPI] {method} {route}");
        StartCoroutine(CallApiCoroutine<T>(www, callback));
    }

    /* -------------------------------------------------------------------------- */

    private IEnumerator CallApiCoroutine<T>(UnityWebRequest www, Action<bool, string, T> callback)
    {
        // WebAPI        
        if(Application.isEditor)
            www.SetRequestHeader("cookie", _debugCookie);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        // results
        bool isSuccess = false;
        string msg = "";
        T returnData;

        try
        {
            ApiResultModel resultModel = JsonUtility.FromJson<ApiResultModel>(www.downloadHandler.text);
                    
            Debug.Log($"<color=teal>[WebAPI] {www.method} {www.url} > </color>\n{www.downloadHandler.text}");

            isSuccess = resultModel.isSuccess;
            msg = resultModel.message;
            // to T
            returnData = (T)resultModel.data;            
        }
        catch(Exception e)
        {
            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"[WebAPI] Server Response with Error: {www.error} [{www.url}]\n(ERR={e})\n(Resp={www.downloadHandler.text})");
                callback?.Invoke(false, $"伺服器發生錯誤 (Err {www.responseCode})", default(T));
            }
            else
            {
                Debug.LogError($"[WebAPI] Parse response Error: [{www.url}] ERR={e}");
                callback?.Invoke(false, "解析資料發生錯誤 (Err 000)", default(T));
            }
            yield break;
        }
        callback?.Invoke(isSuccess, msg, returnData);        
    }
    

    /* -------------------------------------------------------------------------- */


    /// <summary>
    /// Wrapper of API result
    /// You can change this to fit your api model
    /// </summary>
    private class ApiResultModel
    {
        public bool isSuccess {get; set;}
        public string message {get; set;}
        public object data {get; set;}
    }
}