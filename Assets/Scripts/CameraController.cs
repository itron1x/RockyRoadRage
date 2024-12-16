using UnityEngine;

public class CameraController : MonoBehaviour{
    public GameObject player;
    private Vector3 _offset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
       _offset = transform.position - player.transform.position; 
    }

    void LateUpdate(){
        transform.position = player.transform.position + _offset;
    }
}
