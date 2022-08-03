using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace JC.TrashGameToolkit.Sample
{
    public class ResultUI : PopUI
    {
        [SerializeField] Text _scoreText;
        [SerializeField] Text _timeText;
        [SerializeField] Text _gateText;
        [SerializeField] GameObject _mainPanel;
        [SerializeField] GameObject _logPanel;
        [SerializeField] GameObject _logTemplate;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            _mainPanel.SetActive(true);
            _logPanel.SetActive(false);
            _logTemplate.SetActive(false);
            base.Awake();
        }

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
            _scoreText.text = "0";
            _timeText.text =  "0";
            _gateText.text =  "0";
            foreach(var s in score.GatePassed)
            {
                var log = Instantiate(_logTemplate, _logTemplate.transform.parent);
                log.SetActive(true);
                log.GetComponentInChildren<Text>().text = s.ToString();
            }

            Show();

            OnShowAnimFinished += ()=>{
                DOTween.To(t=>{
                    _scoreText.text = (score.MaxScore * t).ToString("0");
                    _timeText.text = (score.SurvivalTime * t).ToString("0.0");
                    _gateText.text = (score.GatePassed.Count * t).ToString("0");
                }, 0, 1, 1.25f);
            };
        }

        public void OpenLeaderboard()
        {
            PromptBox.CreateMessageBox("TODO");
        }

        public void ExitToMenu()
        {
            GameManager.Instance.GoScene("JC_Landing");
        }

        public void ToggleMenu()
        {
            _mainPanel.SetActive(!_mainPanel.activeSelf);
            _logPanel.SetActive(!_logPanel.activeSelf);
        }
    }
}
