using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;

namespace JC.TrashGameToolkit.Sample
{
    public static class MathUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static double DoMath(String expr)
        {
            expr = expr.Replace("×", "*").Replace("÷", "/");
            try
            {
                var result = Convert.ToDouble(new DataTable().Compute(expr, null));
                return result;
            }
            catch(Exception e)
            {
                Debug.LogError($"{e.Message}: {expr}");
                return 1;
            }
        }



        /// <summary>
        /// Make a random math question with the given difficulty.
        /// </summary>
        /// <param name="complexity"></param>
        /// <returns></returns>
        public static string MakeMath(int complexity)
        {
            if (complexity <= 0)
                throw new Exception("Error Complexity " + complexity);

            List<string> operators = new List<string> { 
                "+", "+", "+", 
                "-", "-", "-", 
                "×",
                "÷",
                "(" 
            };
            bool isOpValid = false;
            List<string> opResult = new List<string>();

            // make math opeators
            while (!isOpValid)
            {
                opResult.Clear();
                // int basicOpCount = 0;

                // make math op
                for (int i = 0; i < complexity; i++)
                {
                    string op = "";
                    while (op == "" || (op == "(" && (i == 0 || i >= complexity - 1)))
                    {
                        op = operators.Random();
                    }
                    opResult.Add(op);
                    // if (op != "(")
                    //     basicOpCount++;
                }

                // 補下括號
                for (int i = 0; i < opResult.Count; i++)
                {
                    if (opResult[i] == "(")
                    {
                        opResult.Insert(UnityEngine.Random.Range(i+2, opResult.Count+1), ")");
                    }
                }

                if (opResult[0] == "(")
                    opResult.Insert(0, "+");

                // check valid
                // isOpValid = basicOpCount >= 1;
                isOpValid = true;
            }

            // make math numbers
            string exp = "";
            for (int i = 0; i < opResult.Count; i++)
            {
                exp += opResult[i];
                // if (opResult[i] == ")") continue;
                if (i < opResult.Count - 1 && opResult[i + 1] == "(") continue;

                switch(opResult[i])
                {
                    case "+":
                    case "-":
                        exp += UnityEngine.Random.Range(0, 5);
                        break;
                    case "×":
                    case "(":
                        exp += UnityEngine.Random.Range(0, 3);
                        break;
                    case "÷":
                        exp += UnityEngine.Random.Range(1, 4);
                        break;
                    default:
                        // throw new Exception("Error opResult " + opResult[i]);
                        break;
                }
            }

            // fixme
            exp = exp.Replace(")(", ")×(");

            return exp;
        }
    }
}