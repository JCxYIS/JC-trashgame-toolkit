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
                    "Name: " + data.first_name + " " + data.last_name + "\n" + 
                    "Email: " + data.email + "\n" +
                    "Avatar: " + data.avatar;
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
        WebApiHelper.CallApi<List<ReqresUser>>("GET", "https://reqres.in/api/users", (succ, msg, data)=>
        {
            if (succ)
            {
                _responseText.text = "Total: " + data.Count + "\n";
                foreach (var user in data)
                {
                    _responseText.text += 
                        user.first_name + " " + user.last_name + "\n";
                }
            }
            else
            {
                PromptBox.CreateMessageBox("Error: "+msg);
            }
        });        
    }

    void Post()
    {
        PromptBox.CreateMessageBox("TODO");
    }


    /* -------------------------------------------------------------------------- */

    [System.Serializable]
    public class ReqresUser
    {
        public int id;
        public string email;
        public string first_name;
        public string last_name;
        public string avatar;
    }
}