using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private LapCheckpointTracker _lapCheckpointTracker;
    
    [SerializeField] private int CheckpointId; // ID of checkpoint - checkpoints must be reached in order
    [SerializeField] private bool LapFinish; //Toggles Lap Finish
    [SerializeField] private Transform respawnPoint; // Transform position player respawn
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _lapCheckpointTracker.BallThroughCheckpoint(this, other.transform);
        }
    }

    public void SetLapCheckpointTracker(LapCheckpointTracker lapCheckpointTracker)
    {
        _lapCheckpointTracker = lapCheckpointTracker;
    }

    public int GetCheckpointId()
    {
        return CheckpointId;
    }

    public bool IsLapFinish()
    {
        return LapFinish;
    }

    public Transform GetRespawnPoint()
    {
        return respawnPoint;
    }
}
