using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace JC.TrashGameToolkit.Sample
{
    public static class MathUtil
    {
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
            else if (s.Equals("×")) stack.Push(s);
            else if (s.Equals("÷")) stack.Push(s);
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
            else if (op == "×") result = left * right;
            else if (op == "÷") result = left / right;

            stack.Push(result.ToString());
        }


        return Convert.ToDouble(stack.Pop());
    }



        /// <summary>
        /// Make a random math question with the given difficulty.
        /// </summary>
        /// <param name="complexity"></param>
        /// <returns></returns>
        public static string MakeMath(int complexity)
        {
            if(complexity <= 0)
                throw new Exception("Error Complexity "+complexity);

            List<string> operators = new List<string> { "+", "-",  "×", "÷", "(" };
            bool isOpValid = false;
            List<string> opResult = new List<string>();
            
            // make math opeators
            while(!isOpValid)
            {
                opResult.Clear();
                int basicOpCount = 0;

                // make math op
                for(int i = 0; i < complexity; i++)
                {
                    string op = "";
                    while(op == "" || (op == "(" && i >= complexity - 2))
                    {
                        op = operators.Random();
                    }
                    opResult.Add(op);
                    if(op != "(")
                        basicOpCount++;
                }

                // 補下括號
                for(int i = 0; i < opResult.Count; i++)
                {
                    if(opResult[i] == "(" && basicOpCount > 0)
                    {
                        opResult.Insert(UnityEngine.Random.Range(i+2, opResult.Count+1), ")");
                    }
                }

                if(opResult[0] == "(")
                    opResult.Insert(0, "+");
                
                // check valid
                isOpValid = basicOpCount >= 1;
            }

            // make math numbers
            string exp = "";
            for(int i = 0; i < opResult.Count; i++)
            {
                exp += opResult[i];
                if(opResult[i] == ")") continue;
                if(i < opResult.Count-1 && opResult[i+1] == "(") continue;
                exp += UnityEngine.Random.Range(1, 5);
            }

            return exp;
        }
    }
}