using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GlobalstatsIO;
using System;

/// <summary>
/// Call Init() First!
/// Powered by https://globalstats.io/
/// </summary>
public class LeaderboardService : MonoSingleton<LeaderboardService>
{
    private string GlobalstatsIOApiId = "YourApiIdHere";
    private string GlobalstatsIOApiSecret = "YourApiSecretHere";

    /* -------------------------------------------------------------------------- */

    // The below data will be set during runtime
    public string UserId { get; private set; } = ""; // You can optionally set this to a custom user id.
    public string UserName { get; set; } = ""; // 
    private GlobalstatsIOClient _client;

    /* -------------------------------------------------------------------------- */

    /// <summary>
    /// Initialize the service.
    /// </summary>
    /// <param name="globalStatsApiID"></param>
    /// <param name="globalStatsApiSecret"></param>
    /// <param name="userName"></param>
    /// <param name="userId"></param>
    public void Init(string globalStatsApiID, string globalStatsApiSecret, string userName, string userId = "")
    {
        GlobalstatsIOApiId = globalStatsApiID;
        GlobalstatsIOApiSecret = globalStatsApiSecret;
        UserName = userName;  
        
        // check
        if(string.IsNullOrEmpty(globalStatsApiID))
        {
            Debug.LogError("[LeaderboardService] Init: GlobalstatsIOApiId is empty.");
            return;
        }
        if(string.IsNullOrEmpty(globalStatsApiSecret))
        {
            Debug.LogError("[LeaderboardService] Init: GlobalstatsIOApiSecret is empty.");
            return;
        }
        if(string.IsNullOrEmpty(UserName))
        {
            throw new Exception("UserName is empty. Please set it before submitting a score.");
        }

        if(userId != "" && UserId == "")
            UserId = userId;

        if(string.IsNullOrEmpty(UserId))
        {
            if(PlayerPrefs.GetString("LeaderboardService.UserId", "") != "")
            {
                UserId = PlayerPrefs.GetString("LeaderboardService.UserId");
                Debug.Log($"UserId is empty. Found previous saved id \"{UserId}\"");
            }
            else
            {
                UserId = Guid.NewGuid().ToString();                
                Debug.Log($"UserId is empty. Set with auto-genenrated id \"{UserId}\", saved.");
            }
        }

        PlayerPrefs.SetString("LeaderboardService.UserId", UserId);
        if(_client == null)
            _client = new GlobalstatsIOClient(GlobalstatsIOApiId, GlobalstatsIOApiSecret);

        DontDestroyOnLoad(this);
    }


    /// <summary>
    /// Submit a score to the leaderboard.
    /// </summary>
    /// <param name="scores">A dictionary contains all GTD and values</param>
    /// <param name="callback"></param>
    public void SubmitScore(Dictionary<string, string> scores, Action<bool> callback = null)
    {
        // Put your game's score here, e.g.
        // payload.Add("score", rec.Score.ToString("0"));

        // UserId autogen        

        // Validate
        if(_client == null)
        {
            throw new Exception("Please call Init() before submitting a score.");
        }
        if(scores.Count == 0)
        {
            throw new Exception("The scores you want to submit is empty. Please set it.");
        }

        // Send
        StartCoroutine(_client.Share(
            values: scores, 
            id: UserId, 
            name: UserName,
            callback: null));
    }

    public void GetLeaderboard(string gtd, int numberOfPlayers = 50, Action<Leaderboard> callback = null)
    {
        if(_client == null)
        {
            throw new Exception("Please call Init() before submitting a score.");
        }

        StartCoroutine(_client.GetLeaderboard(
            gtd: gtd,
            numberOfPlayers: numberOfPlayers,
            callback: callback));
    }
}