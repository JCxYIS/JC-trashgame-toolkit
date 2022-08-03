using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace JC.TrashGameToolkit.Sample
{

    public class Player : MonoBehaviour
    {
        [Header("Bindings")]
        public GameObject allyPrefab;
        public MainUI ui;

        [Header("Runtime")]
        public int score = 1;
        [ReadOnly] public GameScore gameScore;
        [ReadOnly][SerializeField] private List<GameObject> allies = new List<GameObject>();


        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            gameScore = new GameScore();
            PlayerController.SpeedBoost = 1;   
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Finish"))
            {
                var wall = other.GetComponent<Wall>();
                other.enabled = false;

                // Make math equation
                string exp = wall.behavior;
                // if(exp.Contains("x")) // algebra: too hard not implemented lol
                //     exp = exp.Replace("x", score.ToString());
                // else
                exp = score + exp;

                // Do the math plz
                var newScoreRaw = MathUtil.DoMath(exp);
                int newScore = (int)Math.Round(newScoreRaw);
                int delta = newScore - score;
                print($"{exp} = {newScore} [{newScoreRaw}]");
                               
                // Apply score
                if(delta > 0)
                {
                    for(int i = 0; i < delta; i++)
                    {
                        var ally = Instantiate(allyPrefab, 
                            transform.position + new Vector3(-1+2f*UnityEngine.Random.value, 0, -1f*UnityEngine.Random.value), 
                            Quaternion.identity);
                        // ally.transform.parent = transform;
                        allies.Add(ally);
                    }
                }
                else
                {
                    for(int i = 0; i < -delta && allies.Count > 0; i++)
                    {
                        Destroy(allies[0]);
                        allies.RemoveAt(0);
                    }
                }                     

                // Invoke Event
                gameScore.PassedGate(score, wall.behavior, newScore);
                score = newScore;
                ui.SetScore(score, delta);
                PlayerController.SpeedBoost += 0.0148763f;
                wall.OnTrigger(delta > 0);    

                // GG
                if(newScore <= 0)
                {
                    print("GG");
                    GameManager.Instance.GameOver(gameScore);
                    gameObject.SetActive(false);
                }  
            }
        }

    
    }
}