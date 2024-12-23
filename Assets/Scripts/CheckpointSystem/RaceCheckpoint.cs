using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private CheckpointTracker checkpointTracker;
    
    [SerializeField] private int CheckpointId; // ID of checkpoint - checkpoints must be reached in order

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointTracker.PlayerThroughCheckpoint(this);
        }
    }

    public void SetCheckpointTracker(CheckpointTracker checkpointTracker)
    {
        this.checkpointTracker = checkpointTracker;
    }

    public int GetCheckpointId()
    {
        return this.CheckpointId;
    }
}
