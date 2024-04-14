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
    public string UserName { get; set; } = ""; // 
    public string UserUrl { get; private set; } = ""; // User's url on the Leaderboard (If not append mode, we will keep update scores on this link)
    private GlobalstatsIOClient _client;

    /* -------------------------------------------------------------------------- */

    /// <summary>
    /// Initialize the service.
    /// </summary>
    /// <param name="globalStatsApiID"></param>
    /// <param name="globalStatsApiSecret"></param>
    public void Init(string globalStatsApiID, string globalStatsApiSecret)
    {
        GlobalstatsIOApiId = globalStatsApiID;
        GlobalstatsIOApiSecret = globalStatsApiSecret;
        
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

        if(_client == null)
            _client = new GlobalstatsIOClient(GlobalstatsIOApiId, GlobalstatsIOApiSecret);

        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Initialize the service (w/ username).
    /// </summary>
    /// <param name="globalStatsApiID"></param>
    /// <param name="globalStatsApiSecret"></param>
    /// <param name="userName"></param>
    /// <param name="userId"></param>
    public void Init(string globalStatsApiID, string globalStatsApiSecret, string userName)
    {
        Init(globalStatsApiID, globalStatsApiSecret);
        SetUserName(userName);
    }

    public void SetUserName(string userName)
    {
        UserName = userName;
        UserUrl = PlayerPrefs.GetString("LeaderboardService.UserUrl", "");
    }


    /// <summary>
    /// Submit a score to the leaderboard.
    /// </summary>
    /// <param name="scores">A dictionary contains all GTD and values</param>
    /// <param name="append">Append new score, or update the previous record of this user?</param>
    /// <param name="callback"></param>
    public void SubmitScore(Dictionary<string, string> scores, bool append = true, Action<bool> callback = null)
    {
        // Put your game's score here, e.g.
        // payload.Add("score", rec.Score.ToString("0"));

        // Editor: Skip submitting score to the leaderboard
        if(Application.isEditor)
        {
            Debug.Log("[LeaderboardService] SubmitScore: Skipped in Editor mode.");
            callback?.Invoke(true);
            return;
        }

        // Validate
        if(_client == null)
        {
            throw new Exception("Please call Init() before submitting a score.");
        }
        if(scores.Count == 0)
        {
            throw new Exception("The scores you want to submit is empty. Please set it.");
        }
        if(string.IsNullOrEmpty(UserName))
        {
            Debug.LogWarning("[LeaderboardService] SubmitScore: UserName is empty.");
        }

        // Send
        StartCoroutine(_client.Share(
            values: scores, 
            id: append ? "" : UserUrl, // if set, upload the score to the specified id
            name: UserName,
            callback: succ=>{
                if(succ)
                    UserUrl = _client.StatisticId;
                callback?.Invoke(succ);
            }));
    }

    /// <summary>
    /// Get the leaderboard with a specific GTD.
    /// </summary>
    /// <param name="gtd">record/score entry</param>
    /// <param name="numberOfPlayers">max players to retrieve</param>
    /// <param name="callback"></param>
    public void GetLeaderboard(string gtd, int numberOfPlayers = 50, Action<Leaderboard> callback = null)
    {
        // if(_client == null)
        // {
        //     throw new Exception("Please call Init() before submitting a score.");
        // }

        StartCoroutine(_client.GetLeaderboard(
            gtd: gtd,
            numberOfPlayers: numberOfPlayers,
            callback: callback));
    }

    /// <summary>
    /// 
    /// </summary>
    // public void GetRecord()
    // {
    //     return _client.GetStatistic();
    // }
}