using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectables{
    public class CoinController : MonoBehaviour{
        private int _coins;
        private TextMeshProUGUI _coinText;
        
        [SerializeField]
        private RaceTelemetry raceTelemetry;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start(){
            _coinText = raceTelemetry.GetCoinText();
            SetCountText();
        }

        void OnTriggerEnter(Collider other){
            if (other.gameObject.CompareTag("Collectables")){
                _coins += 1;
                SetCountText();
                other.gameObject.SetActive(false);
            }
        }

        void SetCountText(){
            _coinText.text = _coins.ToString(); 
        }
    }
}
