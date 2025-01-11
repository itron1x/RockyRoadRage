using Player;
using UnityEngine;

namespace Player_2._0{
    //Extended collision detection for characters
    public class GroundDetection2 : MonoBehaviour{

        private bool _isGrounded;
    
        public PlayerController2 parent;
        
        // Set grounded to true by default.
        void Awake(){
            _isGrounded = false;
            parent.Jump = false;
        }
        
        // Method to call if the character collides with the ground.
        void OnTriggerEnter(Collider other){
            if (other.gameObject.layer == 3) print("Ground Detection");
            if (other.gameObject.layer == 3 && !_isGrounded){
                print("Ground");
                _isGrounded = true;
                parent.Jump = false;
            }
        }

        void OnTriggerStay(Collider other){
            if (other.gameObject.layer == 3){
                print("Stay");
            }
        }
        
        // Method to call if the character collides leaves the ground.
        void OnTriggerExit(Collider other){
            if (other.gameObject.layer == 3) print("Air Detection");
            if (other.gameObject.layer == 3 && _isGrounded){
                print("Jump");
                _isGrounded = false;
            }
        }

        public bool IsGrounded(){
            return _isGrounded;
        }

    }
}
