using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JC.TrashGameToolkit.Sample
{
    public class GameScore
    {
        // public int Score;
        public int MaxScore = 0;
        public List<GateRecord> GatePassed = new List<GateRecord>();
        public float SurvivalTime = 0;
        private float _startTime = 0;

        public class GateRecord
        {
            public int StartScore;
            public string Question;
            public int FinalScore;

            public override string ToString()
            {
                return $"<color=green>{StartScore}</color>{Question} = <color=red><b>{FinalScore}</b></color>";
            }
        }

        public GameScore()
        {
            _startTime = Time.time;
        }

        public void PassedGate(int startValue, string question, int finalValue)
        {
            var record = new GateRecord();
            record.StartScore = startValue;
            record.Question = question;
            record.FinalScore = finalValue;
            GatePassed.Add(record);

            SurvivalTime = Time.time - _startTime;
            if(finalValue > MaxScore)
            {
                MaxScore = finalValue;
            }
        }

    }
}