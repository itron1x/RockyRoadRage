using System;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RaceTelemetry : MonoBehaviour
{
    //TODO: change GameObject maybe to TextMeshProUGUI (less code)? Did it with coinCount
    [SerializeField] private TextMeshProUGUI raceTimer;
    [SerializeField] private TextMeshProUGUI splitsDisplay;
    //TODO: TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] public String playerName;
    
    private RaceControlManager raceControlManager;
    private long _raceStartTimestamp = -1;
    private long _raceEndTimestamp = -1;
    private List<long> _lapSplits;
    private int _playerIndex;
    private bool timerActive = false;
    private Transform _respawnPoint;
    
    public void Awake()
    {
        raceTimer.gameObject.SetActive(false);
        splitsDisplay.gameObject.SetActive(false);
        _lapSplits = new List<long>();
    }

    private void Update()
    {
        if (!timerActive) return;
        long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
        long timeSinceStart = timestampNow - _raceStartTimestamp;
        // Convert to DateTime
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceStart).DateTime;
        raceTimer.text = dateTime.ToString("mm:ss.fff");
    }

    public void SetRaceControlManager(RaceControlManager raceControlManager)
    {
        this.raceControlManager = raceControlManager;
    }
    
    public void SetRaceStartTimestamp(long raceStartTimestamp)
    {
        _raceStartTimestamp = raceStartTimestamp;
        timerActive = true;
        raceTimer.gameObject.SetActive(true);
    }

    public void finish()
    {
        _raceEndTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var duration = _raceEndTimestamp - _raceStartTimestamp;
        DateTime finishTime = DateTimeOffset.FromUnixTimeMilliseconds(duration).DateTime;
        splitsDisplay.text = "Finish! Race Time: " + finishTime.ToString("mm:ss.fff");
        timerActive = false;
        raceTimer.gameObject.SetActive(false);
        
        raceControlManager.PlayerFinishedRace(this);
    }

    public void lapSplit()
    {
        long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
        long timeSinceStart = timestampNow - _raceStartTimestamp;
        _lapSplits.Add(timeSinceStart);
        // Convert to DateTime
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceStart).DateTime;
        splitsDisplay.text = dateTime.ToString("mm:ss.fff");
        splitsDisplay.gameObject.SetActive(true);
        Invoke(nameof(ClearSplits),2);
    }

    public void displayWrongCheckpointWarning()
    {
        splitsDisplay.text = "Wrong Way!";
        splitsDisplay.gameObject.SetActive(true);
        Invoke(nameof(ClearSplits),2);
    }
    private void ClearSplits()
    {
        splitsDisplay.text = "";
        splitsDisplay.gameObject.SetActive(false);
    }

    public List<long> GetLapSplits(){
        return _lapSplits;
    }
    
    public int GetPlayerIndex()
    {
        return _playerIndex;
    }

    public void SetPlayerIndex(int playerIndex)
    {
        _playerIndex = playerIndex;
    }
    public TextMeshProUGUI GetCoinText(){
        return coinText;
    }

    public void SetRespawnPoint(Transform respawnPoint)
    {
        _respawnPoint = respawnPoint;
    }   
    public void Respawn(Rigidbody playerBody)
    {
        playerBody.linearVelocity = Vector3.zero;
        playerBody.angularVelocity = Vector3.zero;
        playerBody.MovePosition(_respawnPoint.position);
    }
}
