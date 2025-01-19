using UnityEngine;

namespace Collectables{
    public class CoinCollider : MonoBehaviour{
        [SerializeField] private CoinController coinController;
        [SerializeField] private AudioClip coinSound;
        
        // When collecting a coin this method is called.
        void OnTriggerEnter(Collider other){
            if (other.gameObject.CompareTag("Collectables")){
                coinController.AddCoins(other);
                SoundManager.Instance.PlaySoundFX(coinSound, 1f, transform);
            }
        }
    }
}
