using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour{
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;
    
    public float speed;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
       _rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void OnMove(InputValue movementInput){
        Vector2 movement = movementInput.Get<Vector2>();

        _movementX = movement.x;
        _movementY = movement.y;
    }

    void FixedUpdate(){
        Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);
       _rb.AddForce(movement * speed);
    }
}
