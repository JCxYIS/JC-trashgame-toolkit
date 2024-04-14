using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JC.TrashGameToolkit.Sample
{
    [CreateAssetMenu(fileName = "Secret", menuName = "JCTTK/Samples/Secret")]
    public class Secret : ScriptableObject
    {
        public string LeaderboardAccessToken;
        public string LeaderboardSecret;
        public string LeaderboardUrl;
    }
}
