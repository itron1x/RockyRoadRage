using CheckpointSystem;
using TMPro;
using UnityEngine;

namespace Player_2._0{
    public class CoinController2 : MonoBehaviour{
        private int _coins;
        private TextMeshProUGUI _coinText;
        
        [SerializeField]
        private RaceTelemetry raceTelemetry;
    
        // Initialize coins
        void Start(){
            _coinText = raceTelemetry.GetCoinText();
            SetCountText();
        }

        // When collecting a coin this method is called.
        void OnTriggerEnter(Collider other){
            if (other.gameObject.CompareTag("Collectables")){
                _coins += 1;
                SetCountText();
                other.gameObject.SetActive(false);
            }
        }

        // Update coin text 
        void SetCountText(){
            _coinText.text = _coins.ToString(); 
        }

        // Remove coins
        public void RemoveCoins(int amount){
            if (_coins - amount >= 0) _coins -= amount;
            else _coins = 0;
            SetCountText();
        }
    }
}
