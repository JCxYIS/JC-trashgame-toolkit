using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;
// using UnityEngine.AddressableAssets;
using LitJson;

/// <summary>
/// Web api handler
/// </summary>
public class WebApiHelper : MonoSingleton<WebApiHelper>
{
    /// <summary>
    /// API 網域
    /// e.g. https://example.com/api/
    /// Note: 如果 CallApi() 直接用絕對網址就不會使用
    /// </summary>
    public static string ROOT_URL = "";

    /// <summary>
    /// 有沒有使用 ApiResponseModel (見最底下) 包裝資料
    /// 如果是 false，那 CallApi 回傳的 msg 就是空的
    /// </summary>
    private static bool USE_API_RESPONSE_MODEL = false;

    
    /// <summary>
    /// Request Headers. (e.g. Autherization, Cookies...)
    /// Will be sent in "Cookie" Header each api calls
    /// </summary>
    public static Dictionary<string, string> RequestHeaders = new Dictionary<string, string>();

    /// <summary>
    /// Request id for debug
    /// </summary>
    private static uint _requestId = 0;


    // WebGL jslibs
    // [DllImport("__Internal")]
    // private static extern string GetRootUrl(); // 注意 CORS


    /* -------------------------------------------------------------------------- */

    protected override void Init()
    {
        DontDestroyOnLoad(gameObject);
    }


    /* -------------------------------------------------------------------------- */

    struct UnityWebRequestWrapper
    {
        public uint id;
        public UnityWebRequest request;
    }

    /// <summary>
    /// 呼叫 API
    /// callback: isSuccess, data
    /// </summary>
    public static void CallApi<T>(string method, string route, Action<bool, T> callback)
    {
        CallApi<T>(method, route, null, (succ, msg, data)=>callback?.Invoke(succ, data));
    }

    /// <summary>
    /// 呼叫 API
    /// callback: isSuccess, msg, data
    /// </summary>
    public static void CallApi<T>(string method, string route, Action<bool, string, T> callback)
    {
        CallApi<T>(method, route, null, callback);
    }
    
    /// <summary>
    /// 呼叫 API 配上 payload
    /// callback: isSuccess, data
    /// </summary>
    public static void CallApi<T>(string method, string route, object payload, Action<bool, T> callback)
    {
        CallApi<T>(method, route, payload, (succ, msg, data)=>callback?.Invoke(succ, data));
    }

    /// <summary>
    /// 呼叫 API 配上 payload
    /// callback: isSuccess, msg, data
    /// </summary>
    public static void CallApi<T>(string method, string route, object payload, Action<bool, string, T> callback)
    {
        // format input
        method = method.ToUpper();
        string url = route.StartsWith("http://") || route.StartsWith("https://") ? 
            route :
            Path.Combine(ROOT_URL, route);

        UnityWebRequest www = new UnityWebRequest(url, method);

        // headers
        foreach(var pair in RequestHeaders)
        {
            www.SetRequestHeader(pair.Key, pair.Value);
        }

        // payload
        int payloadSize = 0;
        if(payload != null)
        {
            string payloadStr = JsonUtility.ToJson(payload);
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(payloadStr);
            payloadSize = postBytes.Length;
            // Debug.Log($"[WebAPI] Request Payload size={payloadSize}\nContent={payloadStr}");
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        }

        // start www
        UnityWebRequestWrapper wrapper = new UnityWebRequestWrapper{id=++_requestId, request=www};
        Debug.Log($"[WebAPI] <color=teal> ({wrapper.id.ToString("000")}) {method} {url} </color>< " + 
            (RequestHeaders.Count == 0 ? "" : $"(Headers: {RequestHeaders.Count} Entries)") +
            (payloadSize == 0 ? "" : $"(Payload: {payloadSize} bytes)")
        );
        Instance.StartCoroutine(Instance.CallApiCoroutine<T>(wrapper, callback));
    }

    /* -------------------------------------------------------------------------- */

    private IEnumerator CallApiCoroutine<T>(UnityWebRequestWrapper wwwWrapper, Action<bool, string, T> callback)
    {
        uint requestId = wwwWrapper.id;
        UnityWebRequest www = wwwWrapper.request;

        // WebAPI
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        // results
        bool isSuccess = false;
        string msg = "";
        T returnData;        

        // parse response
        try
        {
            if(USE_API_RESPONSE_MODEL)
            {
                ApiResponseModel<T> resultModel = JsonMapper.ToObject<ApiResponseModel<T>>(www.downloadHandler.text);  
                returnData = resultModel.data;  
                isSuccess = true;  // isSuccess = resultModel.isSuccess;
                // msg = resultModel.message;
            }
            else
            {
                returnData = JsonMapper.ToObject<T>(www.downloadHandler.text);
                isSuccess = true;
            }
        }
        catch(Exception e)
        {
            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[WebAPI] Server Response with Error: {www.error} [{www.url}]\n(ERR={e})\n(Resp={www.downloadHandler.text})");
                msg = $"伺服器回報錯誤 (Err {www.responseCode})";
            }
            else
            {
                Debug.LogError(e);
                msg = "解析資料發生錯誤";
            }
            returnData = default(T);
        }

        Debug.Log(
            $"[WebAPI] <color=teal>({requestId.ToString("000")}) {www.method} {www.url}</color> > " +
            (isSuccess ? "<color=green>Success</color>" : "<color=red>Failed</color>") + 
            $"({www.responseCode}) " +
            $"{msg}"
            // + $"\n{www.downloadHandler.text}"
            // + $"\n{returnData}"
        );

        callback?.Invoke(isSuccess, msg, returnData);        
    }
    

    /* -------------------------------------------------------------------------- */


    /// <summary>
    /// 如果有使用自定義的 API Response Model 可以在這裡指定
    /// </summary>
    [Serializable]
    private class ApiResponseModel<T>
    {
        // public bool isSuccess {get; set;}
        // public string message {get; set;}
        public T data {get; set;}
    }
}