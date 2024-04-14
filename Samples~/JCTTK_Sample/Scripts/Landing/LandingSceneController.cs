using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JC.TrashGameToolkit.Sample
{
    public class LandingSceneController : MonoBehaviour
    {
        public InputField _nicknameInput;
        public Secret Secret;

        // Start is called before the first frame update
        void Start()
        {
            _nicknameInput.text = PlayerPrefs.GetString("nickname", "");

            if(!Secret)
            {
                Debug.LogError("Secret is not set! Please create a Secret asset and assign it to LandingSceneController");
                return;
            }

            LeaderboardService.Instance.Init(Secret.LeaderboardAccessToken, Secret.LeaderboardSecret);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GoMainScene()
        {
            // check nickname
            if(string.IsNullOrWhiteSpace(_nicknameInput.text))
            {
                PromptBox.CreateMessageBox("Please enter your nickname");
                return;
            }
            PlayerPrefs.SetString("nickname", _nicknameInput.text);

            // load the leaderboard access token
            
            // initialize the leaderboard
            // LeaderboardService.Instance.Init(Secret.LeaderboardAccessToken, Secret.LeaderboardSecret, _nicknameInput.text);
            LeaderboardService.Instance.SetUserName(_nicknameInput.text);

            GameManager.Instance.GoScene("JC_Main"/*, true*/);
        }

        public void OpenLeaderboard()
        {
            // Application.OpenURL(Secret.LeaderboardUrl);
            LeaderboardPanel.Create(new List<string> { "score", "gates", "time" });
        }
    }
}
