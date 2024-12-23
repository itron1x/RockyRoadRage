using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement_old : MonoBehaviour{
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;
    
    public float speed;
    
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI axisText;
    
    private float _horizontalInput;
    private float _verticalInput;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
       _rb = GetComponent<Rigidbody>(); 
       SetPositionText();
       SetAxisText();
    }

    // Update is called once per frame
    void OnMove(InputValue movementInput){
        Vector2 movement = movementInput.Get<Vector2>();

        _movementX = movement.x;
        _movementY = movement.y;
        
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate(){
        Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);
       _rb.AddForce(movement * speed);
       SetPositionText();
    }

    void Update(){
        SetAxisText();
    }

    void SetPositionText(){
        positionText.text = "X: " + transform.position.x + "\nZ: " + transform.position.z;
    }
    void SetAxisText(){
        axisText.text = "Horizontal: " + _horizontalInput + "\nVertical: " + _verticalInput + "\nSpeed: " + speed;
    }
}
