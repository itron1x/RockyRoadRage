using UnityEngine;

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
