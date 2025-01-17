using System;
using System.Collections;
using System.Collections.Generic;
using CheckpointSystem;
using Player_2._0;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaceControlManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private Camera idleCamera;
    [SerializeField] private float joinTimeoutSeconds = 5;
    [SerializeField] private float postRaceTimeoutSeconds = 5;
    [SerializeField] private Canvas leaderboardCanvas;
    [SerializeField] private float raceSpeedMultiplier = 1.5f;
    [SerializeField] private Transform mapOverview;
    
    private List<Transform> _spawnPointLocations = new List<Transform>();
    private List<PlayerInput> _playerInputs = new List<PlayerInput>();
    private List<LeaderBoardEntry> _raceLeaderboard = new List<LeaderBoardEntry>();
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
        RaceInfoSystem infoSystem = RaceInfoSystem.GetInstance();
        if(RaceInfoSystem.GetInstance() == null) return;
        
        // _playerInputManager.playerPrefab = infoSystem.GetPlayerPrefabs()[0];
        // var inputDevice = infoSystem.GetPlayerInputs()[0];
        // print(infoSystem.GetPlayerInputs()[0]);
        raceSpeedMultiplier = infoSystem.GetRaceSpeed();
        // _playerInputManager.JoinPlayer(-1, -1, null, inputDevice);

        //TODO: get input from RaceInfoSystem!
        List<InputDevice> devices = infoSystem.GetPlayerInputs(); 
        for (int i = 0; i < devices.Count; i++){
            _playerInputManager.JoinPlayer(i, controlScheme: null, pairWithDevice: devices[i]);
        }

    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        print("Trigger OnPlayerJoined");
        idleCamera.gameObject.SetActive(false);
        
        ResetWaitTime();
        playerInput.DeactivateInput();
        
        // ? Question = is it necessary to also deactivate the Physics of the gameObject ?
        
        GameObject newPlayer = playerInput.gameObject;
        _playerInputs.Add(playerInput);
        
        int playerIndex = _playerInputs.IndexOf(playerInput);
        
        LapCheckpointTracker lapCheckpointTracker = GetComponent<LapCheckpointTracker>();
        lapCheckpointTracker.AddPlayer(newPlayer.GetComponentInChildren<Rigidbody>().transform);
        
        RaceTelemetry playerRaceTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
        Transform spawnPoint = _spawnPointLocations[playerIndex];
        playerRaceTelemetry.SetRaceControlManager(this);
        playerRaceTelemetry.SetPlayerIndex(playerIndex);
        playerRaceTelemetry.SetRespawnPoint(spawnPoint); //sets initial spawn point
        
        _raceControlUI.DisplayUpdateText("Player " + (playerIndex + 1) + " joined! Race will begin shortly...");
        newPlayer.GetComponentInChildren<Rigidbody>().MovePosition(spawnPoint.position);
        
    }

    private void ActivateRace()
    {
        Time.timeScale = raceSpeedMultiplier;
        _raceStartTimeMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        
        foreach (PlayerInput playerInput in PlayerInput.all){
            RaceTelemetry playerTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
            playerTelemetry.SetRaceStartTimestamp(_raceStartTimeMilliseconds);
            playerInput.ActivateInput();
        }
        // foreach (var playerInput in _playerInputs)
        // {
        //     //TODO: FIX
        //     var playerRaceTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
        //     playerRaceTelemetry.SetRaceStartTimestamp(_raceStartTimeMilliseconds);
        //     playerInput.ActivateInput();
        // }
    }

    public void PlayerFinishedRace(RaceTelemetry playerRaceTelemetry)
    {
        var playerIndex = playerRaceTelemetry.GetPlayerIndex();
        Debug.Log("Player " + playerIndex + " finished!"  + playerRaceTelemetry.GetFinishTime()+"   "+ playerRaceTelemetry.GetPlayerName());
        
        //Switch the player camera to the finish after 2 seconds
        StartCoroutine(PlayerFinishCamera(playerIndex, 2));
        //add the player's time to the race leaderboard
        LeaderBoardEntry finishedPlayer = new LeaderBoardEntry(playerRaceTelemetry.GetFinishTime(), playerRaceTelemetry.GetPlayerName());
        _raceLeaderboard.Add(finishedPlayer);
        //also add it to the global leaderboard
        RaceInfoSystem infoSystem = RaceInfoSystem.GetInstance();
        infoSystem?.AddGlobalLeaderboardEntry(infoSystem.ActiveMapIndex, playerRaceTelemetry.GetFinishTime(),playerRaceTelemetry.GetPlayerName()); //TODO: add dynamic MapIndex
        infoSystem?.AddGlobalCoins(playerRaceTelemetry.getPlayerCoins());
        
        //when all Players have finished the Race, display Leaderboard and save Leaderboard and Coins to disk
        if (_raceLeaderboard.Count >= _playerInputs.Count)
        {
            _raceControlUI.DisplayUpdateText("All Players have finished! Loading leaderboard...");
            SaveSystem.SaveLeaderboard(); //save after Player finished to keep the finish time even if race gets aborted afterwards
            SaveSystem.Save();
            Invoke(nameof(ShowLeaderboard), postRaceTimeoutSeconds);
        }
    }

    IEnumerator PlayerFinishCamera(int playerIndex, float delay){
        yield return new WaitForSeconds(delay);
        
        GameObject playerGameObject = _playerInputs[playerIndex].gameObject;
        Transform parent = playerGameObject.transform.parent;
        PrefabController prefabController = playerGameObject.GetComponentInParent<PrefabController>();
        // Set camera to finish checkpoint
        //parent.GetComponentInChildren<CinemachineCamera>().Follow = mapOverview;
        
        prefabController.GetCinemachineCamera().Follow = mapOverview;
        // Make player invisible
        prefabController.GetCharacter().layer = LayerMask.NameToLayer("Invisible");
        prefabController.GetOverlays().layer = LayerMask.NameToLayer("Invisible");
        prefabController.GetEye().layer = LayerMask.NameToLayer("Invisible");
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

    private void ShowLeaderboard()
    {
        LeaderboardScript leaderboardScript = leaderboardCanvas.GetComponent<LeaderboardScript>();
        leaderboardScript.SetRaceLeaderboard(_raceLeaderboard);
        leaderboardScript.ShowRaceLeaderboard();
        idleCamera.gameObject.SetActive(true);
        leaderboardCanvas.gameObject.SetActive(true);
        _raceControlUI.gameObject.SetActive(false);
    }

    public void SetRaceSpeedMultiplier(float multiplier)
    {
        raceSpeedMultiplier = multiplier;
    }
    
}
