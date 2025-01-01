using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class RaceControlManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private Camera idleCamera;
    [SerializeField] private float joinTimeoutSeconds = 5;
    
    private List<Vector3> _spawnPointLocations = new List<Vector3>();
    private List<PlayerInput> _playerInputs = new List<PlayerInput>();
    private RaceControlUI _raceControlUI;
    private long _raceStartTimeMilliseconds;
    
    private IEnumerator _countdown;
    
    private void Awake()
    {
        _raceControlUI = GetComponent<RaceControlUI>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.CompareTag("Spawn"))
            {   
                Vector3 spawnLocation = spawnPoint.transform.position;
                Debug.Log("Found spawn point " + spawnPoint.name + " at " + spawnLocation);
                
                _spawnPointLocations.Add(spawnLocation);
                
            }
        }
    }
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        idleCamera.gameObject.SetActive(false);
        ResetWaitTime();
        playerInput.DeactivateInput();
        
        GameObject newPlayer = playerInput.gameObject;
        _playerInputs.Add(playerInput);
        
        int playerIndex = _playerInputs.IndexOf(playerInput);
        CheckpointTracker raceControl = GetComponent<CheckpointTracker>();
        raceControl.AddPlayer(newPlayer.transform);
        Vector3 spawnPoint = _spawnPointLocations[playerIndex];
        _raceControlUI.DisplayUpdateText("Player " + (playerIndex + 1) + " joined! Race will begin shortly...");
        newPlayer.GetComponent<Rigidbody>().MovePosition(spawnPoint); //whenever an object has a Rigidbody, it needs to be moved this way to integrate with the physics engine.
        
    }

    private void ActivateRace()
    {
        _raceStartTimeMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        foreach (var playerInput in _playerInputs)
        {
            RaceTelemetry playerRaceTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
            playerRaceTelemetry.SetRaceStartTimestamp(_raceStartTimeMilliseconds);
            playerInput.ActivateInput();
        }
    }
    
    private void ResetWaitTime()
    {
        if (_countdown != null) StopCoroutine(_countdown);
        _countdown = WaitBeforeCountdown(joinTimeoutSeconds);
        StartCoroutine(_countdown);
    }

    // every 2 seconds perform the print()
    private IEnumerator StartRaceWithCountdown()
    {
        _raceControlUI.ClearUpdateText();
        for (var i = 3; i >= 1; i--)
        {
            _raceControlUI.DisplayCountdownText("" + i);
            yield return new WaitForSeconds(1f);
        }
        _raceControlUI.DisplayCountdownText("Roll!", 2);
        ActivateRace();
    }

    private IEnumerator WaitBeforeCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return StartRaceWithCountdown();
    }

}
