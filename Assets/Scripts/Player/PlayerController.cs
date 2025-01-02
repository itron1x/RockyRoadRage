using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
    public class PlayerController : MonoBehaviour{
        private Rigidbody _rb;
        private RaceTelemetry _raceTelemetry;
    
        //Character Settings
        public float speed;
        public float jumpForce;
        public bool jump;
    
        [SerializeField]
        private GroundDetection groundDetection;

        [SerializeField] private Transform mainCamera;
    
        //Movement Coordinates
        private float _movementX;
        private float _movementY;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start(){
            _rb = GetComponent<Rigidbody>(); 
            _raceTelemetry = transform.parent.GetComponentInChildren<RaceTelemetry>();
            Debug.Log(_raceTelemetry);
        }

        void Awake(){
            Transform parent = transform.parent;
            // mainCamera = parent.GetComponentInChildren<Camera>().transform;
        }
        
        void FixedUpdate(){
            Vector3 movement = new Vector3(_movementX, 0.0f, _movementY); 
            movement = Quaternion.AngleAxis(mainCamera.rotation.eulerAngles.y, Vector3.up) * movement;
        
            if(!jump && !(_rb.linearVelocity.magnitude > speed)) _rb.AddForce(movement * speed, ForceMode.Acceleration);
        }

        // Update is called once per frame
        void OnMove(InputValue movementInput){
            Vector2 movement = movementInput.Get<Vector2>();

            _movementX = movement.x;
            _movementY = movement.y;
        }

        void OnJump(){
            Vector3 jumpVector = new Vector3(0.0f, jumpForce, 0.0f);
            if (groundDetection.IsGrounded){
                _rb.AddForce(jumpVector,ForceMode.Impulse);
                jump = true;
            }
        }

        void OnInteract()
        {
            Debug.Log("Respawning");
            _raceTelemetry.Respawn(_rb);
        }

        void OnApplicationFocus(bool focus){
            if (focus){
                Cursor.lockState = CursorLockMode.Locked;
            }
            else{
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
