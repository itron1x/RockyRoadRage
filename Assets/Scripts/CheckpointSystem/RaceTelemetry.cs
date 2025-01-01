using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceTelemetry : MonoBehaviour
{
    [SerializeField] private GameObject raceTimer;
    [SerializeField] private GameObject splitsDisplay;
    [SerializeField] public String playerName;
    
    private RaceControlManager raceControlManager;
    private long _raceStartTimestamp = -1;
    private long _raceEndTimestamp = -1;
    private List<long> _lapSplits;
    private int _playerIndex;
    private bool timerActive = false;
    
    private TextMeshProUGUI _timerText;
    private TextMeshProUGUI _splitText;

    public void Awake()
    {
        raceTimer.SetActive(false);
        splitsDisplay.SetActive(false);
        _timerText = raceTimer.GetComponent<TextMeshProUGUI>();
        _splitText = splitsDisplay.GetComponent<TextMeshProUGUI>();
        _lapSplits = new List<long>();
    }

    private void Update()
    {
        if (!timerActive) return;
        long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
        long timeSinceStart = timestampNow - _raceStartTimestamp;
        // Convert to DateTime
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceStart).DateTime;
        _timerText.text = dateTime.ToString("mm:ss.fff");
        raceTimer.SetActive(true);
    }

    public void SetRaceControlManager(RaceControlManager raceControlManager)
    {
        this.raceControlManager = raceControlManager;
    }
    
    public void SetRaceStartTimestamp(long raceStartTimestamp)
    {
        _raceStartTimestamp = raceStartTimestamp;
        timerActive = true;
    }

    public void finish()
    {
        _raceEndTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var duration = _raceEndTimestamp - _raceStartTimestamp;
        DateTime finishTime = DateTimeOffset.FromUnixTimeMilliseconds(duration).DateTime;
        _splitText.text = "Finish! Race Time: " + finishTime.ToString("mm:ss.fff");
        timerActive = false;
        raceTimer.SetActive(false);
        raceControlManager.PlayerFinishedRace(this);
    }

    public void lapSplit()
    {
        long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
        long timeSinceStart = timestampNow - _raceStartTimestamp;
        _lapSplits.Add(timeSinceStart);
        // Convert to DateTime
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceStart).DateTime;
        _splitText.text = dateTime.ToString("mm:ss.fff");
        splitsDisplay.SetActive(true);
        Invoke(nameof(ClearSplits),2);
    }

    public void displayWrongCheckpointWarning()
    {
        _splitText.text = "Wrong Way!";
        splitsDisplay.SetActive(true);
        Invoke(nameof(ClearSplits),2);
    }
    private void ClearSplits()
    {
        _splitText.text = "";
        splitsDisplay.SetActive(false);
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
    
}
