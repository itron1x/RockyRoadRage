using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    
    public Transform checkpointsParent; //Parent Object containing all Checkpoints as children
    
    private int _nextCheckpoint = 0;
    private int _checkpointCount = 0;
    
    private void Awake()
    {
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

    public void PlayerThroughCheckpoint(Checkpoint checkpoint)
    {
        int passedCheckpoint = checkpoint.GetCheckpointId();
        if(passedCheckpoint == _nextCheckpoint-1) return; // Player went through same checkpoint twice
        
        if (passedCheckpoint == _nextCheckpoint)
        {
            _nextCheckpoint = (_nextCheckpoint + 1) % _checkpointCount;
            Debug.Log("Player passed Checkpoint " + checkpoint.GetCheckpointId() + ". Next Checkpoint: "+ _nextCheckpoint);
        }
        else
        {
            Debug.Log("Player passed wrong Checkpoint " + checkpoint.GetCheckpointId());
        }
    }
    
}
