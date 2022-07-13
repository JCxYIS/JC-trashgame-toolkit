using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WebApiTest : MonoBehaviour
{
    [SerializeField] InputField _input;
    [SerializeField] Button _getButton;
    [SerializeField] Button _getListButton;
    [SerializeField] Button _postButton;
    [SerializeField] Text _responseText;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _getButton.onClick.AddListener(()=>Get(_input.text));
        _getListButton.onClick.AddListener(GetList);
        _postButton.onClick.AddListener(Post);
    }

    void Get(string id)
    {
        string url =  $"https://reqres.in/api/users/{id}";
        WebApiHelper.CallApi<ReqresUser>("GET", url, (succ, msg, data)=>
        {
            if (succ)
            {
                _responseText.text = 
                    "GET From URL=" + url + "\n"  +
                    "Name: " + data.data.first_name + " " + data.data.last_name + "\n" + 
                    "Email: " + data.data.email + "\n" +
                    "Avatar: " + data.data.avatar;
            }
            else
            {
                PromptBox.CreateMessageBox("Error: "+msg);
            }
        });

        // Debug.Log(testJson);
        // Debug.Log(JsonUtility.FromJson<ReqresUser>(testJson).data.avatar);
        // Debug.Log(JsonUtility.FromJson<ReqresUser>(testJson).support.url);

        // Debug.Log(JsonUtility.FromJson<ReqresUser[]>(testJsonArr)[2].data.avatar);
    }

    void GetList()
    {
        var result = JsonUtility.FromJson<ReqresUser[]>(testJsonArr);
        foreach (var item in result)
        {
            Debug.Log(item.data.avatar);
        }
    }

    void Post()
    {

    }


    /* -------------------------------------------------------------------------- */

    string testJson = @"{ ""data"": { ""id"": 1, ""email"": ""george.bluth@reqres.in"", ""first_name"": ""George"", ""last_name"": ""Bluth"", ""avatar"": ""https://reqres.in/img/faces/1-image.jpg"" }, ""support"": { ""url"": ""https://reqres.in/#support-heading"", ""text"": ""To keep ReqRes free, contributions towards server costs are appreciated!"" } }";   
    string testJsonArr = @"[ { ""id"": 1, ""email"": ""george.bluth@reqres.in"", ""first_name"": ""George"", ""last_name"": ""Bluth"", ""avatar"": ""https://reqres.in/img/faces/1-image.jpg"" }, { ""id"": 2, ""email"": ""janet.weaver@reqres.in"", ""first_name"": ""Janet"", ""last_name"": ""Weaver"", ""avatar"": ""https://reqres.in/img/faces/2-image.jpg"" }, { ""id"": 3, ""email"": ""emma.wong@reqres.in"", ""first_name"": ""Emma"", ""last_name"": ""Wong"", ""avatar"": ""https://reqres.in/img/faces/3-image.jpg"" } ]";   

    [System.Serializable]
    public class ReqresUser
    {
        public ReqresUserData data;  // strong typing
        public dynamic support;  // dynamic typing (check project setting, .Net version need to set to 4.X, and backend should only be mono)
    } 

    [System.Serializable]
    public class ReqresUserData
    {
        public int id;
        public string email;
        public string first_name;
        public string last_name;
        public string avatar;
    }
}