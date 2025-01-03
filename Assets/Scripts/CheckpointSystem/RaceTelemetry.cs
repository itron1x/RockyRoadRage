using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using TMPro;
using UnityEngine;

namespace CheckpointSystem{
    public class RaceTelemetry : MonoBehaviour
    {
        [Header("Timer")]
        [SerializeField] private TextMeshProUGUI raceTimer;
        [SerializeField] private TextMeshProUGUI splitsDisplay;
    
        [Header("Out of bounds")]
        [SerializeField] private TextMeshProUGUI outOfBoundsDisplay;
        [SerializeField] private int outOfBoundsRespawnTimerSeconds = 3;

        [Header("Other")] 
        [SerializeField] private CoinController coinController;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] public String playerName;

        private RaceControlManager raceControlManager;
        private long _raceStartTimestamp = -1;
        private long _raceEndTimestamp = -1;
        private List<long> _lapSplits;
        private int _playerIndex;
        private bool timerActive = false;
        private Transform _respawnPoint;
    
        private Coroutine _respawnTimerCoroutine;

        public void Awake()
        {
            raceTimer.gameObject.SetActive(false);
            splitsDisplay.gameObject.SetActive(false);
            outOfBoundsDisplay.gameObject.SetActive(false);
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
            Invoke(nameof(ClearSplits), 2);
        }

        public void displayWrongCheckpointWarning()
        {
            splitsDisplay.text = "Wrong Way!";
            splitsDisplay.gameObject.SetActive(true);
            Invoke(nameof(ClearSplits), 2);
        }

        private void ClearSplits()
        {
            splitsDisplay.text = "";
            splitsDisplay.gameObject.SetActive(false);
        }

        public List<long> GetLapSplits()
        {
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

        public TextMeshProUGUI GetCoinText()
        {
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
            coinController.RemoveCoins(2);
        }

        private IEnumerator RespawnTimer( Rigidbody playerBody)
        {
            outOfBoundsDisplay.gameObject.SetActive(true);
            for (var i = outOfBoundsRespawnTimerSeconds; i > 0; i--)
            {
                outOfBoundsDisplay.text = "Respawning in: " + i + "s";
                yield return new WaitForSeconds(1f);
            }
            Respawn(playerBody);
            outOfBoundsDisplay.gameObject.SetActive(false);
        
        }

        public void OnPlayerOutOfBounds(Rigidbody playerBody)
        {
            _respawnTimerCoroutine = StartCoroutine(RespawnTimer(playerBody));
        }

        public void OnPlayerBackInBounds()
        {
            if(_respawnTimerCoroutine != null) StopCoroutine(_respawnTimerCoroutine);
            _respawnTimerCoroutine = null;
        }
    }
}
