using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace JC.TrashGameToolkit.Sample
{

    public class Player : MonoBehaviour
    {
        [Header("Bindings")]
        public int score = 1;
        public GameObject allyPrefab;

        [Header("Runtime")]
        [ReadOnly][SerializeField] private List<GameObject> allies = new List<GameObject>();

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
                if(exp.Contains("x"))
                    exp = exp.Replace("x", score.ToString());
                else
                    exp = score + exp;

                // Do the math plz
                var resultRaw = DoMath(exp);
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
                    Destroy(gameObject);
                }      

                // 
                score = result;
                wall.OnTrigger(delta > 0);          
            }
        }

    // 2+(100/5)+10 = 32
    //((2.5+10)/5)+2.5 = 5
    // (2.5+10)/5+2.5 = 1.6666
    public static double DoMath(String expr)
    {

        Stack<String> stack = new Stack<String>();

        string value = "";
        for (int i = 0; i < expr.Length; i++)
        {
            String s = expr.Substring(i, 1);
            char chr = s.ToCharArray()[0];

            if (!char.IsDigit(chr) && chr != '.' && value != "")
            {
                stack.Push(value);
                value = "";
            }

            if (s.Equals("(")) {

                string innerExp = "";
                i++; //Fetch Next Character
                int bracketCount=0;
                for (; i < expr.Length; i++)
                {
                    s = expr.Substring(i, 1);

                    if (s.Equals("("))
                        bracketCount++;

                    if (s.Equals(")"))
                        if (bracketCount == 0)
                            break;
                        else
                            bracketCount--;


                    innerExp += s;
                }

                stack.Push(DoMath(innerExp).ToString());

            }
            else if (s.Equals("+")) stack.Push(s);
            else if (s.Equals("-")) stack.Push(s);
            else if (s.Equals("*")) stack.Push(s);
            else if (s.Equals("/")) stack.Push(s);
            else if (s.Equals("sqrt")) stack.Push(s);
            else if (s.Equals(")"))
            {
            }
            else if (char.IsDigit(chr) || chr == '.')
            {
                value += s;

                if (value.Split('.').Length > 2)
                    throw new Exception("Invalid decimal.");

                if (i == (expr.Length - 1))
                    stack.Push(value);

            }
            else
                throw new Exception("Invalid character.");

        }


        double result = 0;
        while (stack.Count >= 3)
        {

            double right = Convert.ToDouble(stack.Pop());
            string op = stack.Pop();
            double left = Convert.ToDouble(stack.Pop());

            if (op == "+") result = left + right;
            else if (op == "+") result = left + right;
            else if (op == "-") result = left - right;
            else if (op == "*") result = left * right;
            else if (op == "/") result = left / right;

            stack.Push(result.ToString());
        }


        return Convert.ToDouble(stack.Pop());
    }
    }
}