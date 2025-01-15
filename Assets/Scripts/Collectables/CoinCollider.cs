using CheckpointSystem;
using Player_2._0;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectables{
    public class CoinCollider : MonoBehaviour{
        [SerializeField] private CoinController coinController;

        // When collecting a coin this method is called.
        void OnTriggerEnter(Collider other){
            if (other.gameObject.CompareTag("Collectables")){
                coinController.AddCoins(other);
            }
        }
    }
}
