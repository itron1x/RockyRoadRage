using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour{
    private Rigidbody _rb;
    
    //Character Settings
    public float speed;
    public float jumpForce;
    public bool jump;
    
    [SerializeField]
    private GroundDetection groundDetection;
    private Camera _playerCamera;
    
    //Movement Coordinates
    private float _movementX;
    private float _movementY;
    
    //Moues Coordinates
    
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI mouseText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
       _rb = GetComponent<Rigidbody>(); 
       SetPositionText();
    }

    // Update is called once per frame
    void OnMove(InputValue movementInput){
        Vector2 movement = movementInput.Get<Vector2>();

        _movementX = movement.x;
        _movementY = movement.y;
    }

    void FixedUpdate(){
        Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);
       if(!jump && !(_rb.linearVelocity.magnitude > speed)) _rb.AddForce(movement * speed);
       SetPositionText();
       SetSpeedText();
       SetMouseText();
    }

    void OnJump(){
        Vector3 jumpVector = new Vector3(0.0f, jumpForce, 0.0f);
        if (groundDetection.IsGrounded){
            _rb.AddForce(jumpVector,ForceMode.Impulse);
            jump = true;
        }
    }

    void SetPositionText(){
        positionText.text = "X: " + transform.position.x + "\nZ: " + transform.position.z;
    }
    
    void SetSpeedText(){
        speedText.text = "Speed: " + _rb.linearVelocity.magnitude;
    }

    void SetMouseText(){
        mouseText.text = "Mouse: \n" + "x: " + Mouse.current.position.x.ReadValue() + "y: " + Mouse.current.position.y.ReadValue(); 
    }
}
