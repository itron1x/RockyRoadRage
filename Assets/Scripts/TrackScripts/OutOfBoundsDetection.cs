using CheckpointSystem;
using UnityEngine;

namespace TrackScripts{
    public class OutOfBoundsDetection : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                RaceTelemetry playerRaceTelemetry = other.transform.parent.parent.GetComponentInChildren<RaceTelemetry>();
                if(playerRaceTelemetry.GetFinishTime() == -1) playerRaceTelemetry.OnPlayerOutOfBounds(other.transform.GetComponent<Rigidbody>());
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                RaceTelemetry playerRaceTelemetry = other.transform.parent.parent.GetComponentInChildren<RaceTelemetry>();
                playerRaceTelemetry.OnPlayerBackInBounds();
            }
        }
    
    }
}
