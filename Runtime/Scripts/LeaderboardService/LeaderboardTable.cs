using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardTable : MonoBehaviour
{
    [SerializeField] LeaderboardItem _leaderboardItemTemplate;

    protected void Awake()
    {
        _leaderboardItemTemplate.gameObject.SetActive(false);
    }

    public void SetLeaderboard(string gtd, int numberOfPlayers = 50)
    {   
        // rm old
        foreach (Transform child in _leaderboardItemTemplate.transform.parent)
        {
            if(child == _leaderboardItemTemplate.transform)
                continue;
            Destroy(child.gameObject);
        }
        
        Loading.Instance.SetLoading(true, "Loading Leaderboard...");
        LeaderboardService.Instance.GetLeaderboard(gtd, numberOfPlayers, table=>{
            Loading.Instance.SetLoading(false, "Loading Leaderboard...");
            foreach (var entry in table.data)
            {
                // print($"{entry.rank} | {entry.name} {entry.user_profile} ({entry.user_icon}) {entry.value}");
                var lbItem = Instantiate(_leaderboardItemTemplate, _leaderboardItemTemplate.transform.parent);
                lbItem.gameObject.SetActive(true);
                lbItem.SetData(entry.rank.ToString(), entry.name, entry.user_icon, entry.value);
            }
        });
    }
}