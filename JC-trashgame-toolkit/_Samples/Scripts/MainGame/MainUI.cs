using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace JC.TrashGameToolkit.Sample
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] Text _scoreText;
        [SerializeField] Text _scoreDeltaText;
        [SerializeField] Text _timeText;


        // Start is called before the first frame update
        void Start()
        {
            _scoreText.text = "1";
            _scoreDeltaText.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetScore(int score, int scoreDelta)
        {
            _scoreText.text = score.ToString();
            var delta = Instantiate(_scoreDeltaText, transform);
            delta.gameObject.SetActive(true);
            delta.GetComponent<Text>().text = scoreDelta >= 0 ? "+"+scoreDelta : scoreDelta.ToString();
            delta.GetComponent<Text>().DOFade(0, .2f).SetDelay(.3f);
            delta.GetComponent<RectTransform>().DOAnchorPosY(-75, .5f);
            Destroy(delta, 2);
        }
    }
}
