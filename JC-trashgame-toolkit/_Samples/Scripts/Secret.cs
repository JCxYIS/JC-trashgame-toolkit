using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JC.TrashGameToolkit.Sample
{
    public class Secret 
    {
        // If your repo is not public, you can hard-code the secrets below
        public string LeaderboardAccessToken;
        public string LeaderboardSecret;
        public string LeaderboardUrl;
    }



    public class SecretHandler
    {
        static Secret _secret;

        public static Secret Secret
        {
            get
            {
                if (_secret == null)
                {
                    var secretJson = Resources.Load<TextAsset>("secret");
                    if(string.IsNullOrEmpty(secretJson.text))
                    {
                        Debug.LogWarning("[SecretHandler] Secret is empty, will use Secret() constructor instead");
                        _secret = new Secret();
                    }
                    else
                    {
                        _secret = JsonUtility.FromJson<Secret>(secretJson.text);
                    }
                }
                return _secret;
            }
        }
    }
}
