using UnityEngine;

namespace Player{
    public class PlayerCharacteristics : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GroundDetection groundDetection;
    
        [Header("Movement attributes")] 
        [SerializeField] private int speed;
        [SerializeField] private int acceleration;
        [SerializeField] private int jumpHeight;
        [SerializeField] private float mass;

        public Rigidbody GetRigidbody(){
            return rb;
        }

        public GroundDetection GetGroundDetection(){
            return groundDetection;
        }

        public int GetSpeed(){
            return speed; 
        }

        public int GetAcceleration(){
            return acceleration;
        }

        public int GetJumpHeight(){
            return jumpHeight;
        }

        public float GetMass(){
            return mass;
        }

    }
}
