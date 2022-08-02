using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JC.TrashGameToolkit.Sample
{
    public class StageController : MonoBehaviour
    {
        [Header("Bindings")]
        public GameObject _stagePrefab;

        [Header("Settings")]
        public Vector3 _firstStagePosition;
        public float _stageLength;
        
        // [Header("Questions")]
        // public List<string> _questions;
        Player _player;

        private Queue<GameObject> _stages = new Queue<GameObject>();
        int _stageNo = 0;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            for(int i = 0; i < 2; i++)
            {
                CreateNextStage();
            }

            // TEST MATH
            // for(int i = 0; i < 30; i++)
            // {
            //     string q = "3" + MathUtil.MakeMath(3);
            //     double a = MathUtil.DoMath(q);
            //     print($"{q} = {a}");
            // }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentStageIndex">To confirm</param>
        public void CreateNextStage()
        {
            _stageNo++;

            // Create Stage
            GameObject stage = _stages.Count > 4 ? _stages.Dequeue() : Instantiate(_stagePrefab, transform.position, Quaternion.identity);
            stage.transform.position = _firstStagePosition + Vector3.forward * _stageLength * _stageNo;
            _stages.Enqueue(stage);

            // Make math questions
            List<string> questions = new List<string>();
            Wall[] walls = stage.GetComponentsInChildren<Wall>();
            int playerScore = _player.score;
            for(int i = 0; i < walls.Length; i++)
            {
                for(int attempt = 0; attempt < 10; attempt++) // attempt to make questions
                {
                    string q = MathUtil.MakeMath(playerScore / 10 + 1);
                    double delta = MathUtil.DoMath(playerScore + q) - playerScore;
                    if(
                        (i == 0 && delta > 0 && delta < 30)|| // first wall must be good, but not too much
                        (i != 0 && delta < 0)    // other walls must be bad
                        )
                    {
                        questions.Add(q);
                        walls[i].SetBehavior(q);
                        break;
                    }
                }
            }
            questions.Shuffle();

            // Set walls
            for(int i = 0; i < walls.Length; i++)
            {
                walls[i].stageController = this;
                walls[i].behavior = questions[i];
            }
        }        
    }
}