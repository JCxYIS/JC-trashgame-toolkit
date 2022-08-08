using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JC.TrashGameToolkit.Sample
{
    public class LandingSceneController : MonoBehaviour
    {
        public InputField _nicknameInput;

        // Start is called before the first frame update
        void Start()
        {
            _nicknameInput.text = PlayerPrefs.GetString("nickname", "");
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
            var secret = SecretHandler.Secret;
            
            // initialize the leaderboard
            LeaderboardService.Instance.Init(secret.LeaderboardAccessToken, secret.LeaderboardSecret, _nicknameInput.text);

            GameManager.Instance.GoScene("JC_Main"/*, true*/);
        }

        public void OpenLeaderboard()
        {
            Application.OpenURL(SecretHandler.Secret.LeaderboardUrl);
        }
    }
}
