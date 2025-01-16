using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CheckpointSystem{
    public class RaceTelemetry : MonoBehaviour
    {
        [Header("Timer")]
        [SerializeField] private TextMeshProUGUI raceTimer;
        [SerializeField] private TextMeshProUGUI splitsDisplay;
    
        [Header("Out of bounds")]
        [SerializeField] private TextMeshProUGUI outOfBoundsDisplay;
        [SerializeField] private int outOfBoundsRespawnTimerSeconds = 3;

        [FormerlySerializedAs("coinController")]
        [Header("Other")] 
        [SerializeField] private CoinController coinCollider;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private string playerName;
        [SerializeField] private TextMeshProUGUI playerNameText;

        private RaceControlManager _raceControlManager;
        private long _raceStartTimestamp = -1;
        private long _raceEndTimestamp = -1;
        private List<long> _lapSplits;
        private int _playerIndex;
        private bool _timerActive;
        private Transform _respawnPoint;
    
        private Coroutine _respawnTimerCoroutine;

        public void Awake()
        {
            raceTimer.gameObject.SetActive(false);
            splitsDisplay.gameObject.SetActive(false);
            outOfBoundsDisplay.gameObject.SetActive(false);
            _lapSplits = new List<long>();
            playerNameText.text = playerName;
        }

        private void Update()
        {
            if (!_timerActive) return;
            long timestampNow = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since epoch
            long timeSinceStart = timestampNow - _raceStartTimestamp;
            // Convert to DateTime
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceStart).DateTime;
            raceTimer.text = dateTime.ToString("mm:ss.fff");
        }

        public void SetRaceControlManager(RaceControlManager raceControlManager)
        {
            this._raceControlManager = raceControlManager;
        }

        public void SetRaceStartTimestamp(long raceStartTimestamp)
        {
            _raceStartTimestamp = raceStartTimestamp;
            _timerActive = true;
            raceTimer.gameObject.SetActive(true);
        }

        public void SetPlayerName(string newPlayerName)
        {
            playerName = newPlayerName;
            playerNameText.text = playerName;
        }

        public string GetPlayerName()
        {
            return playerName;
        }

        public int getPlayerCoins()
        {
            return coinCollider.GetCoins();
        }
        
        public void Finish()
        {
            _raceEndTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var duration = _raceEndTimestamp - _raceStartTimestamp;
            DateTime finishTime = DateTimeOffset.FromUnixTimeMilliseconds(duration).DateTime;
            splitsDisplay.text = "Finish! Race Time: " + finishTime.ToString("mm:ss.fff");
            _timerActive = false;
            raceTimer.gameObject.SetActive(false);

            _raceControlManager.PlayerFinishedRace(this);
        }

        public void LapSplit()
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

        public void DisplayWrongCheckpointWarning()
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

        public long GetFinishTime()
        {
            if (_raceEndTimestamp == -1) return -1;
            return _raceEndTimestamp - _raceStartTimestamp;
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
            coinCollider.RemoveCoins(2);
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
            outOfBoundsDisplay.gameObject.SetActive(false);
        }
    }
}
