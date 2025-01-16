using System.Collections.Generic;
using CheckpointSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class LapCheckpointTracker : MonoBehaviour
{

    [SerializeField] private Transform checkpointsParent; //Parent Object containing all Checkpoints as children
    [SerializeField] private List<Transform> playerBallsTransforms;
    [SerializeField] private int lapsToFinish;
    [SerializeField] private List<TMPro.TextMeshProUGUI> checkpointTexts;
    
    private List<int> _nextCheckpointList;
    private List<int> _lapCountList;
    private int _checkpointCount = 0;

    private void Awake()
    {
        _nextCheckpointList = new List<int>();
        _lapCountList = new List<int>();
        foreach (Transform checkpointSingleTransform in checkpointsParent)
        {
            if (checkpointSingleTransform.CompareTag("Checkpoint"))
            {
                Checkpoint checkpoint = checkpointSingleTransform.GetComponent<Checkpoint>();
                checkpoint.SetLapCheckpointTracker(this);
                _checkpointCount++;
            }
        }
    }

    public void BallThroughCheckpoint(Checkpoint checkpoint, Transform ball)
    {
        if (!ball.CompareTag("Player")) return;
        Debug.Log("Detected player ball: " + ball.name);
        
        int playerIndex = playerBallsTransforms.IndexOf(ball);
        int passedCheckpoint = checkpoint.GetCheckpointId();
        int nextCheckpoint = _nextCheckpointList[playerIndex];
        
        if (passedCheckpoint == nextCheckpoint - 1) return; // Player went through same checkpoint twice
        
        RaceTelemetry playerRaceTelemetry = ball.parent.parent.GetComponentInChildren<RaceTelemetry>();
        if (passedCheckpoint == nextCheckpoint)
        {
            _nextCheckpointList[playerIndex] = (nextCheckpoint + 1) % _checkpointCount;
            Debug.Log("Player " + playerIndex+ " passed Checkpoint " + checkpoint.GetCheckpointId());
            playerRaceTelemetry.SetRespawnPoint(checkpoint.GetRespawnPoint());
            
            // **Update Checkpoint Display**
            // UpdateCheckpointDisplay(playerIndex, _nextCheckpointList[playerIndex]);
            
            if (checkpoint.IsLapFinish())
            {
                _lapCountList[playerIndex]++;
                if( _lapCountList[playerIndex] > 0) playerRaceTelemetry.LapSplit(); //ignore the first time over the finish line
                Debug.Log("Player " + playerIndex + " finished lap " + _lapCountList[playerIndex]);
            }
            if( _lapCountList[playerIndex] > 0 && _lapCountList[playerIndex] >= lapsToFinish){
                _lapCountList[playerIndex] = -1; // avoid multiple triggers of finishing
                playerRaceTelemetry.Finish();
            }
        }
        else
        {
            Debug.Log("Player " + playerIndex+ " passed wrong Checkpoint " + checkpoint.GetCheckpointId());
            playerRaceTelemetry.DisplayWrongCheckpointWarning();
        }
        
    }
    
    // **Checkpoint Display**
    // private void UpdateCheckpointDisplay(int playerIndex, int nextCheckpoint)
    // {
    //     int currentCheckpoint = nextCheckpoint == 0 ? _checkpointCount : nextCheckpoint;
    //     checkpointTexts[playerIndex].text = $"Player {playerIndex + 1}: {currentCheckpoint}/{_checkpointCount}";
    // }

    public void AddPlayer(Transform playerBall)
    {
        playerBallsTransforms.Add(playerBall); // Add the players ball to the list so we know which rock is what player
        _nextCheckpointList.Add(0); // Add a checkpoint counter for each player added
        _lapCountList.Add(-1); // Add a lap counter for each player added, and set -1 so passing the finish line at the start doesn't count as a lap yet
        Debug.Log("Added new player " + playerBall.name);
    }
}
