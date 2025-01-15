using System;
using CheckpointSystem;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player_2._0{
    public class PlayerController2 : MonoBehaviour{
        private Rigidbody _rb;
        private RaceTelemetry _raceTelemetry;
    
        //Character Settings
        [Header("Movement attributes")]
        
        private int _acceleration = 20;
        private int _speed = 20;
        private int _jumpForce = 10;
        private float _mass = 1;
        [NonSerialized] public bool Jump;
    
        [Header("References")]
        
        [SerializeField] private Transform mainCamera;
        
        [Header("Changing References")]
        [SerializeField] private GroundDetection2 groundDetection;
    
        //Movement Coordinates
        private float _movementX;
        private float _movementY;
    
        void Awake(){
            _rb = GetComponentInParent<PrefabController>().GetCharacter().GetComponent<Rigidbody>();
            _rb.mass = _mass;
            _raceTelemetry = transform.parent.GetComponentInChildren<RaceTelemetry>();
        }
        
        // Add force based on direction and wanted movement
        void FixedUpdate(){
            Vector3 movement = new Vector3(_movementX, 0.0f, _movementY); 
            movement = Quaternion.AngleAxis(mainCamera.rotation.eulerAngles.y, Vector3.up) * movement;
        
            if(!Jump && !(_rb.linearVelocity.magnitude > _speed)) _rb.AddForce(movement * _acceleration, ForceMode.Acceleration);
        }

        // Change movement for next frame.
        void OnMove(InputValue movementInput){
            Vector2 movement = movementInput.Get<Vector2>();

            _movementX = movement.x;
            _movementY = movement.y;
        }

        // Jump with the character
        void OnJump(){
            Vector3 jumpVector = new Vector3(0.0f, _jumpForce, 0.0f);
            if (groundDetection.IsGrounded()){
                _rb.AddForce(jumpVector,ForceMode.Impulse);
                Jump = true;
            }
        }

        // Manual respawn
        void OnInteract()
        {
            _raceTelemetry.Respawn(_rb);
        }

        // Lock cursor mode on game focus
        void OnApplicationFocus(bool focus){
            if (focus){
                Cursor.lockState = CursorLockMode.Locked;
            }
            else{
                Cursor.lockState = CursorLockMode.None;
            }
        }
        
        public void SetRigidbody(Rigidbody rb){
            _rb = rb;
        }

        public Rigidbody GetRigidbody(){
            return _rb;
        }

        public void SetGroundDetection(GroundDetection2 newGroundDetection){
            groundDetection = newGroundDetection;
        }

        public void SetSpeed(int newSpeed){
            _speed = newSpeed;
        }

        public void SetAcceleration(int newAcceleration){
            _acceleration = newAcceleration;
        }

        public void SetJumpForce(int newJump){
            _jumpForce = newJump;
        }

        public void SetMass(float newMass){
            _mass = newMass;
        }
    }
}
