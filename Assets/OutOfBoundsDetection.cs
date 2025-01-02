using System;
using UnityEngine;

public class OutOfBoundsDetection : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaceTelemetry playerRaceTelemetry = other.transform.parent.GetComponentInChildren<RaceTelemetry>();
            playerRaceTelemetry.OnPlayerOutOfBounds(other.transform.GetComponent<Rigidbody>());
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaceTelemetry playerRaceTelemetry = other.transform.parent.GetComponentInChildren<RaceTelemetry>();
            playerRaceTelemetry.OnPlayerBackInBounds();
        }
    }
    
}
