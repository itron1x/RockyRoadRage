using UnityEngine;

namespace Player{
    public class GroundDetection : MonoBehaviour{

        private bool _isGrounded;
        public bool IsGrounded => _isGrounded; //Make available for Parent
    
        public PlayerController parent;

        void Start(){
            _isGrounded = true;
        }
        void OnTriggerEnter(Collider other){
            print(other.GetType());
            if (other.gameObject.layer == 3 && !_isGrounded){
                _isGrounded = true;
                parent.jump = false;
            }
        }
        void OnTriggerExit(Collider other){
            if (other.gameObject.layer == 3 && _isGrounded){
                _isGrounded = false;
            }
        }

    }
}
