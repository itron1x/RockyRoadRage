using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class CheckpointTracker : MonoBehaviour
{

    [SerializeField] private Transform checkpointsParent; //Parent Object containing all Checkpoints as children
    [SerializeField] private List<Transform> playerBallsTransforms;

    private List<int> _nextCheckpointList;
    private int _checkpointCount = 0;

    private void Awake()
    {
        _nextCheckpointList = new List<int>();
        foreach (Transform checkpointSingleTransform in checkpointsParent)
        {
            if (checkpointSingleTransform.CompareTag("Checkpoint"))
            {
                Checkpoint checkpoint = checkpointSingleTransform.GetComponent<Checkpoint>();
                checkpoint.SetCheckpointTracker(this);
                _checkpointCount++;
            }
        }
    }

    public void BallThroughCheckpoint(Checkpoint checkpoint, Transform ball)
    {
        if (!ball.CompareTag("Player")) return;
        Debug.Log("Detected player ball: " + ball.name);
        
        int playerIndex = playerBallsTransforms.IndexOf(ball);
        Debug.Log(playerIndex);
        
        int passedCheckpoint = checkpoint.GetCheckpointId();
        int _nextCheckpoint = _nextCheckpointList[playerIndex];
        
        if (passedCheckpoint == _nextCheckpoint - 1) return; // Player went through same checkpoint twice

        if (passedCheckpoint == _nextCheckpoint)
        {
            _nextCheckpointList[playerIndex] = (_nextCheckpoint + 1) % _checkpointCount;
            Debug.Log("Player " + playerIndex+ " passed Checkpoint " + checkpoint.GetCheckpointId());
        }
        else
        {
            Debug.Log("Player " + playerIndex+ " passed wrong Checkpoint " + checkpoint.GetCheckpointId());
        }
        
    }

    public void AddPlayer(Transform playerBall)
    {
        playerBallsTransforms.Add(playerBall); // Add the players ball to the list so we know which rock is what player
        _nextCheckpointList.Add(0); // Add a lap counter for each player added
        Debug.Log("Added new player " + playerBall.name);
        Debug.Log(playerBall.name +" " + playerBallsTransforms.Count);
    }
}
