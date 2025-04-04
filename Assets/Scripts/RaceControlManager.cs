using System;
using System.Collections;
using System.Collections.Generic;
using CheckpointSystem;
using Player;
using Sound;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaceControlManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private Camera idleCamera;
    [SerializeField] private float joinTimeoutSeconds = 5;
    [SerializeField] private float postRaceTimeoutSeconds = 5;
    [SerializeField] private Canvas leaderboardCanvas;
    [SerializeField] private Canvas pauseMenuCanvas;
    [SerializeField] private float raceSpeedMultiplier = 1.5f;
    [SerializeField] private Transform mapOverview;
    
    [Header("Music")]
    [SerializeField] private AudioClip raceMusic;
    [SerializeField] private AudioClip postRaceMusic;
    [SerializeField] private AudioClip preRaceMusic;
    [SerializeField] private AudioClip beepLow;
    [SerializeField] private AudioClip beepHigh;
    [SerializeField] private AudioSource musicSource;
    
    private AudioSource _currentMusic;
    
    private List<Transform> _spawnPointLocations = new List<Transform>();
    private List<PlayerInput> _playerInputs = new List<PlayerInput>();
    private List<LeaderBoardEntry> _raceLeaderboard = new List<LeaderBoardEntry>();
    private RaceControlUI _raceControlUI;
    private long _raceStartTimeMilliseconds;
    
    private IEnumerator _PreRaceCountdown;
    private PlayerInputManager _playerInputManager;
    private RaceInfoSystem _raceInfoSystem;

    private bool _isPaused = false;
    private long _pauseStartTimestamp;
    private long _pauseEndTimestamp;
    private long _totalPauseTime = 0;
    public long TotalPauseTime => _totalPauseTime;
    public bool IsPaused => _isPaused;

    private void Awake()
    {
        _raceInfoSystem = RaceInfoSystem.GetInstance();
        _raceInfoSystem.ActiveRaceControlManager = this;
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
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        if(RaceInfoSystem.GetInstance() == null) return;
        
        raceSpeedMultiplier = _raceInfoSystem.GetRaceSpeed();

        List<InputDevice> devices = _raceInfoSystem.GetPlayerInputs();
        for (int i = 0; i < devices.Count; i++){
            _playerInputManager.JoinPlayer(i, controlScheme: null, pairWithDevice: devices[i]);
        }

        switch (_raceInfoSystem.GetPlayerInputs().Count){
            case 3:
                idleCamera.gameObject.SetActive(true);
                idleCamera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                break;
            case 5:
                idleCamera.gameObject.SetActive(true);
                idleCamera.rect = new Rect(0.66f, 0f, 0.34f, 0.5f);
                break;
            case 7:
                idleCamera.gameObject.SetActive(true);
                idleCamera.rect = new Rect(0.75f, 0f, 0.25f, 0.5f);
                break;
            default:
                idleCamera.gameObject.SetActive(false);
                break;
        }
        SwitchMusic(preRaceMusic);
        
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        print("Trigger OnPlayerJoined");
        // idleCamera.gameObject.SetActive(false);
        
        ResetWaitTime();
        playerInput.DeactivateInput();
        
        // ? Question = is it necessary to also deactivate the Physics of the gameObject ?
        
        GameObject newPlayer = playerInput.gameObject;
        _playerInputs.Add(playerInput);
        
        PrefabController prefabController = playerInput.gameObject.transform.parent.GetComponent<PrefabController>();
        prefabController.SetCharacter(GetCharacter(_raceInfoSystem.GetPlayerCharacter()[playerInput.playerIndex]), _raceInfoSystem.GetPlayerName()[playerInput.playerIndex]);
        
        int playerIndex = _playerInputs.IndexOf(playerInput);
        
        LapCheckpointTracker lapCheckpointTracker = GetComponent<LapCheckpointTracker>();
        lapCheckpointTracker.AddPlayer(newPlayer.GetComponentInChildren<Rigidbody>().transform);
        
        RaceTelemetry playerRaceTelemetry = playerInput.gameObject.transform.parent.GetComponentInChildren<RaceTelemetry>();
        Transform spawnPoint = _spawnPointLocations[playerIndex];
        playerRaceTelemetry.SetRaceControlManager(this);
        playerRaceTelemetry.SetPlayerIndex(playerIndex);
        playerRaceTelemetry.SetRespawnPoint(spawnPoint); //sets initial spawn point
        
        _raceControlUI.DisplayUpdateText("Race will begin shortly...");
        newPlayer.GetComponentInChildren<Rigidbody>().MovePosition(spawnPoint.position);
        
    }

    private void ActivateRace()
    {
        PlayMusic();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
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
        Debug.Log("Player " + playerIndex + " finished!"  + playerRaceTelemetry.GetFinishTime()+"   "+ playerRaceTelemetry.GetPlayerName());
        
        //Switch the player camera to the finish after 2 seconds
        StartCoroutine(PlayerFinishCamera(playerIndex, 2));
        
        //add the player's time to the race leaderboard
        LeaderBoardEntry finishedPlayer = new LeaderBoardEntry(playerRaceTelemetry.GetFinishTime(), playerRaceTelemetry.GetPlayerName());
        _raceLeaderboard.Add(finishedPlayer);
        
        //also add it to the global leaderboard
        _raceInfoSystem?.AddGlobalLeaderboardEntry(_raceInfoSystem.ActiveMapIndex, playerRaceTelemetry.GetFinishTime(),playerRaceTelemetry.GetPlayerName()); //TODO: add dynamic MapIndex
        _raceInfoSystem?.AddGlobalCoins(playerRaceTelemetry.getPlayerCoins());
        
        //when all Players have finished the Race, display Leaderboard and save Leaderboard and Coins to disk
        if (_raceLeaderboard.Count >= _playerInputs.Count)
        {
            idleCamera.gameObject.SetActive(true);
            idleCamera.rect = new Rect(0, 0, 1, 1);
            
            _raceControlUI.DisplayUpdateText("All Players have finished! Loading leaderboard...");
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
        StopMusic();
        StageMusic(raceMusic);
        _raceControlUI.ClearUpdateText();
        for (var i = 3; i >= 1; i--)
        {
            SoundManager.Instance.PlaySoundFX(beepLow, 1f, transform);
            _raceControlUI.DisplayCountdownText("" + i);
            yield return new WaitForSeconds(1f);
        }
        SoundManager.Instance.PlaySoundFX(beepHigh,1f, transform);
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SwitchMusic(postRaceMusic);
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

    public string GetCharacter(int characterIndex){
        switch (characterIndex){
            case 0:
                return "Pebble Pete";
            case 1:
                return "Cubic Chris";
            case 2:
                return "Triangle Tam";
            case 3:
                return "Smooth Sally";
            case 4:
                return "Lava Larry";
        }
        throw new NotImplementedException("Character not found.");
    }

    public void togglePause()
    {
        if(_isPaused) resumeRace();
        else pauseRace();
    }

    public void pauseRace(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _currentMusic.Pause();
        _isPaused = true;
        _pauseStartTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Time.timeScale = 0;
        pauseMenuCanvas.gameObject.SetActive(true);
    }
    public void resumeRace()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentMusic.Play();
        _pauseEndTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        _totalPauseTime += _pauseEndTimestamp - _pauseStartTimestamp;
        pauseMenuCanvas.gameObject.SetActive(false);
        Time.timeScale = 1 * raceSpeedMultiplier;
        _isPaused = false;
    }

    private void SwitchMusic(AudioClip audioClip)
    {
        if (_currentMusic != null)
        {
            _currentMusic.Stop();
            Destroy(_currentMusic);
        }
        _currentMusic = Instantiate(musicSource,transform.position, Quaternion.identity);
        _currentMusic.clip = audioClip;
        _currentMusic.volume = 1f;
        _currentMusic.loop = true;
        _currentMusic.Play();
    }

    private void StageMusic(AudioClip audioClip)
    {
        if (_currentMusic != null)
        {
            _currentMusic.Stop();
            Destroy(_currentMusic);
        }
        _currentMusic = Instantiate(musicSource,transform.position, Quaternion.identity);
        _currentMusic.clip = audioClip;
        _currentMusic.volume = 1f;
        _currentMusic.loop = true;
    }

    private void PlayMusic()
    {
        _currentMusic.Play();
    }

    private void StopMusic()
    {
        _currentMusic?.Stop();
        Destroy(_currentMusic);
    }
}
