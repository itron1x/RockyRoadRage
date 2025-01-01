using System;
using TMPro;
using UnityEngine;

public class RaceTelemetry : MonoBehaviour
{
    [SerializeField] private GameObject raceTimer;
    [SerializeField] private GameObject splitsDisplay;
    
    private const int TOTAL_LAPS_NEEDED = 3;
    
    private long _raceStartTimestamp = -1;
    private long _raceEndTimestamp = -1;
    
    private TextMeshProUGUI _timerText;
    private TextMeshProUGUI _splitText;

    public void Awake()
    {
        raceTimer.SetActive(false);
        splitsDisplay.SetActive(false);
        _timerText = raceTimer.GetComponent<TextMeshProUGUI>();
        _splitText = splitsDisplay.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_raceStartTimestamp > 0)
        {
            long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
            long timeSinceStart = timestampNow - _raceStartTimestamp;
            // Convert to DateTime
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceStart).DateTime;
            _timerText.text = dateTime.ToString("mm:ss.fff");
            raceTimer.SetActive(true);
        }
    }

    public void SetRaceStartTimestamp(long raceStartTimestamp)
    {
        _raceStartTimestamp = raceStartTimestamp;
    }

    public void displaySplit()
    {
        long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
        long timeSinceStart = timestampNow - _raceStartTimestamp;
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
    
}
