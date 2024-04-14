using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardPanel : PopUI
{    
    [SerializeField] LeaderboardTable _leaderboardTable;    
    [SerializeField] LeaderboardGtdButtons _leaderboardGtdButtons;    

    protected override void Awake()
    {
        base.Awake();
    }

    /* -------------------------------------------------------------------------- */
    
    public static void Create(List<string> gtds, int numberOfPlayers = 50)
    {
        var g = ResourcesUtil.InstantiateFromResourcesJCTTK("Prefabs/LeaderboardPanel");
        var obj = g.GetComponent<LeaderboardPanel>();
        obj.Show();
        obj._leaderboardGtdButtons.OnGtdButtonClicked.AddListener(gtd =>
        {
            obj._leaderboardTable.SetLeaderboard(gtd, numberOfPlayers);
        });
        obj._leaderboardGtdButtons.SetGtds(gtds);
    }
}