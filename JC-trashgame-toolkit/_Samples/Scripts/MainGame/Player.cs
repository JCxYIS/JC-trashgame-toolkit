using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace JC.TrashGameToolkit.Sample
{

    public class Player : MovingObject
    {
        [Header("Bindings")]
        public GameObject allyPrefab;
        public MainUI ui;

        [Header("Runtime")]
        [ReadOnly] public GameScore gameScoreRecorder;
        [ReadOnly][SerializeField] private List<GameObject> allies = new List<GameObject>();
        public int score => allies.Count;
        public int gatePassed => gameScoreRecorder.GatePassed.Count;


        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            gameScoreRecorder = new GameScore();
            PlayerController.SpeedBoost = 1;   
            allies.Add(gameObject);
        }

        protected override void KillSelf()
        {
            // base.KillSelf();
            ApplyScore(0);
        }

        public void KillAlly(GameObject ally, bool showDeltaOnUi = true)
        {
            allies.Remove(ally);
            Destroy(ally);
            if(showDeltaOnUi)
                RefreshUi(-1);
        }

        void ApplyScore(int newScore)
        {
            int delta = newScore - score;

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
                    if(allies.Count > 1)
                        KillAlly(allies[1], false);
                    else
                        KillAlly(allies[0], false);
                }
            }      

            RefreshUi(delta);      
        }

        void RefreshUi(int delta)
        {
            // set
            ui.SetScore(score, delta);

            // GG
            if(score <= 0)
            {
                print("GG");
                GameManager.Instance.GameOver(gameScoreRecorder);
                gameObject.SetActive(false);
            }
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
                

                // Invoke Event
                gameScoreRecorder.PassedGate(score, wall.behavior, newScore);                
                PlayerController.SpeedBoost += 0.0148763f;
                wall.OnTrigger(delta > 0);    

                ApplyScore(newScore);                                                       
            }
        }

    
    }
}