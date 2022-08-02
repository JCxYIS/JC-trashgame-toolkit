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
        public float survivalTime = 0;
        public int wallPassed = 0;
        [ReadOnly][SerializeField] private List<GameObject> allies = new List<GameObject>();

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            survivalTime += Time.deltaTime;
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

                // Make math equation
                string exp = wall.behavior;
                // if(exp.Contains("x")) // algebra: too hard not implemented lol
                //     exp = exp.Replace("x", score.ToString());
                // else
                exp = score + exp;

                // Do the math plz
                var resultRaw = MathUtil.DoMath(exp);
                int result = (int)Math.Round(resultRaw);
                int delta = result - score;
                print($"{exp} = {result} ({resultRaw})");
                               
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

                // GG
                if(result <= 0)
                {
                    print("GG");
                    gameObject.SetActive(false);
                }      

                // 
                score = result;
                wall.OnTrigger(delta > 0);    
                ui.SetScore(score, delta);
            }
        }

    
    }
}