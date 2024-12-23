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
        _nextCheckpointList.Add(0);
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
        int passedCheckpoint = checkpoint.GetCheckpointId();
        int _nextCheckpoint = _nextCheckpointList[0];
        
        if (passedCheckpoint == _nextCheckpoint - 1) return; // Player went through same checkpoint twice

        if (passedCheckpoint == _nextCheckpoint)
        {
            _nextCheckpointList[0] = (_nextCheckpoint + 1) % _checkpointCount;
            Debug.Log("Player passed Checkpoint " + checkpoint.GetCheckpointId());
        }
        else
        {
            Debug.Log("Player passed wrong Checkpoint " + checkpoint.GetCheckpointId());
        }
        
    }

    public void addPlayer(Transform playerBall)
    {
        playerBallsTransforms.Add(playerBall);
        
    }
}
