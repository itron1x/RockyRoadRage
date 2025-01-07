using UnityEngine;

namespace Player{
    //Extended collision detection for characters
    public class GroundDetection : MonoBehaviour{

        private bool _isGrounded;
        public bool IsGrounded => _isGrounded; //Make available for Parent
    
        public PlayerController parent;

        // Set grounded to true by default.
        void Start(){
            _isGrounded = true;
        }
        
        // Method to call if the character collides with the ground.
        void OnTriggerEnter(Collider other){
            if (other.gameObject.layer == 3 && !_isGrounded){
                _isGrounded = true;
                parent.Jump = false;
            }
        }
        
        // Method to call if the character collides leaves the ground.
        void OnTriggerExit(Collider other){
            if (other.gameObject.layer == 3 && _isGrounded){
                _isGrounded = false;
            }
        }

    }
}
