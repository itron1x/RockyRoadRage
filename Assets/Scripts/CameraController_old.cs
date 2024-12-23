using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController_old : MonoBehaviour{
    public GameObject player;
    private Vector3 _offset;
    
    //Mouse Position
    private Quaternion _rotation;
    private float _mouseY;
    private float _mouseX;
    private float _mouseYClamped;
    private float _mouseXClamped;

    [SerializeField] private float xRotateMin;
    [SerializeField] private float xRotateMax;
    [SerializeField] private float yRotateMin;
    [SerializeField] private float yRotateMax;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
       // _offset = transform.position - player.transform.position; 
       Cursor.visible = false;
       Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        _mouseX = Mouse.current.position.x.ReadValue() - Screen.width / 2;
        _mouseY = Mouse.current.position.y.ReadValue() - Screen.height / 2;
    }

    void LateUpdate(){
        
        _mouseYClamped = Mathf.Clamp(_mouseY,yRotateMin,yRotateMax);
        _mouseXClamped = Mathf.Clamp(_mouseX,xRotateMin,xRotateMax);
        _rotation = Quaternion.Euler(_mouseYClamped, _mouseXClamped, 0.0f);
        
        // transform.position = player.transform.position - _rotation * _offset + Vector3.up * 10;
        transform.position = player.transform.position - _rotation * new Vector3(0,0,7) + Vector3.up * 1;
        transform.rotation = _rotation;
    }
}
