using UnityEngine;
using System.Collections;

public enum LeaderboardFilterType
{
    FRIENDS = 0,
    ALL = 1,
    MAX
};

[System.Serializable]
public class LeaderboardsController {

    public UILeaderboardController ui;
    public GameDataController localDataController;
    
    public IEnumerator LoadToUI(World world, LeaderboardFilterType filter)
    {
        WorldRecordData worldUserData = localDataController.GetWorldRecord(world.name);

        ui.SetAsLoading();
        ui.ClearScoresList();
        if (worldUserData != null)
            ui.PushScore(1, "YOU", worldUserData.topSpeed, worldUserData.topTime);
        else
            ui.PushScore(-1, "No scores available", 0.0f, 0.0f);
        ui.ShowScores();
        yield return true;
    }
}
