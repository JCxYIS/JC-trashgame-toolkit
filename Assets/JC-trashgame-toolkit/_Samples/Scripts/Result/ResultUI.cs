using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JC.TrashGameToolkit.Sample
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] Text _scoreText;
        [SerializeField] Text _timeText;
        [SerializeField] Text _gateText;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Show(GameScore score)
        {
            _scoreText.text = score.MaxScore.ToString();
            _timeText.text = score.SurvivalTime.ToString();
            _gateText.text = score.GatePassed.ToString();
            gameObject.SetActive(true);
        }

        public void OpenLeaderboard()
        {
            PromptBox.CreateMessageBox("TODO");
        }

        public void ExitToMenu()
        {
            GameManager.Instance.GoScene("JC_Landing");
        }
    }
}
