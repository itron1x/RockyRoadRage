using System;
using System.Collections;
using System.Collections.Generic;
using CheckpointSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class RaceControlManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private Camera idleCamera;
    [SerializeField] private float joinTimeoutSeconds = 5;
    [SerializeField] private float postRaceTimeoutSeconds = 5;
    [SerializeField] private Canvas leaderboardCanvas;
    [SerializeField] private float raceSpeedMultiplier = 1.5f;
    
    private List<Transform> _spawnPointLocations = new List<Transform>();
    private List<PlayerInput> _playerInputs = new List<PlayerInput>();
    private List<int> _leaderboard = new List<int>();
    private RaceControlUI _raceControlUI;
    private long _raceStartTimeMilliseconds;
    
    private IEnumerator _PreRaceCountdown;
    private PlayerInputManager _playerInputManager;
    
    private void Awake()
    {
        leaderboardCanvas.gameObject.SetActive(false);
        _playerInputManager = GetComponent<PlayerInputManager>();
        _raceControlUI = GetComponent<RaceControlUI>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.CompareTag("Spawn"))
            {   
                Transform spawnLocation = spawnPoint.transform;
                Debug.Log("Found spawn point " + spawnPoint.name + " at " + spawnLocation);
                
                _spawnPointLocations.Add(spawnLocation);
                
            }
        }
    }

    private void Start()
    {
        if(RaceInfoSystem.GetInstance() == null) return;
        
        _playerInputManager.playerPrefab = RaceInfoSystem.GetInstance().GetPlayerPrefabs()[0];
        var inputDevice = RaceInfoSystem.GetInstance().GetPlayerInputs()[0];
        
        _playerInputManager.JoinPlayer(-1, -1, null, inputDevice);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        idleCamera.gameObject.SetActive(false);
        
        ResetWaitTime();
        playerInput.DeactivateInput();
        
        // ? Question = is it necessary to also deactivate the Physics of the gameObject ?
        
        GameObject newPlayer = playerInput.gameObject;
        _playerInputs.Add(playerInput);
        
        int playerIndex = _playerInputs.IndexOf(playerInput);
        
        LapCheckpointTracker lapCheckpointTracker = GetComponent<LapCheckpointTracker>();
        lapCheckpointTracker.AddPlayer(newPlayer.transform);
        
        RaceTelemetry playerRaceTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
        Transform spawnPoint = _spawnPointLocations[playerIndex];
        playerRaceTelemetry.SetRaceControlManager(this);
        playerRaceTelemetry.SetPlayerIndex(playerIndex);
        playerRaceTelemetry.SetRespawnPoint(spawnPoint); //sets initial spawn point
        
        _raceControlUI.DisplayUpdateText("Player " + (playerIndex + 1) + " joined! Race will begin shortly...");
        newPlayer.GetComponent<Rigidbody>().MovePosition(spawnPoint.position); //whenever an object has a Rigidbody, it needs to be moved this way to integrate with the physics engine.
    }

    private void ActivateRace()
    {
        Time.timeScale = raceSpeedMultiplier;
        _raceStartTimeMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        foreach (var playerInput in _playerInputs)
        {
            var playerRaceTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
            playerRaceTelemetry.SetRaceStartTimestamp(_raceStartTimeMilliseconds);
            playerInput.ActivateInput();
        }
    }

    public void PlayerFinishedRace(RaceTelemetry playerRaceTelemetry)
    {
        var playerIndex = playerRaceTelemetry.GetPlayerIndex();
        Debug.Log("Player " + playerIndex + " finished!");
        _playerInputs[playerIndex].DeactivateInput();
        _leaderboard.Add(playerIndex);
        
        //when all Players have finished the Race, display Leaderboard
        if (_leaderboard.Count >= _playerInputs.Count)
        {
            _raceControlUI.DisplayUpdateText("All Players have finished! Loading leaderboard...");
            Invoke(nameof(showLeaderboard), postRaceTimeoutSeconds);
        }
        
    }

    private void ResetWaitTime()
    {
        if (_PreRaceCountdown != null) StopCoroutine(_PreRaceCountdown);
        _PreRaceCountdown = WaitBeforeCountdown(joinTimeoutSeconds);
        StartCoroutine(_PreRaceCountdown);
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
        _playerInputManager.DisableJoining(); //Lock new players out before the countdown starts
        yield return StartRaceWithCountdown();
    }

    private void showLeaderboard()
    {
        LeaderboardScript leaderboardScript = leaderboardCanvas.GetComponent<LeaderboardScript>();
        for (int i = 1; i <= _leaderboard.Count; i++)
        {
            var playerRaceTelemetry = _playerInputs[i - 1].gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
            DateTime finishTime = DateTimeOffset.FromUnixTimeMilliseconds(playerRaceTelemetry.GetFinishTime()).DateTime;
            leaderboardScript.SetPlayer(i, playerRaceTelemetry.GetPlayerName(), finishTime.ToString("mm:ss.fff"), playerRaceTelemetry.GetCoinText().text);
        }
        
        leaderboardCanvas.gameObject.SetActive(true);
        _raceControlUI.gameObject.SetActive(false);
    }

    public void setRaceSpeedMultiplier(float multiplier)
    {
        raceSpeedMultiplier = multiplier;
    }
    
}
