using UnityEngine;

public class IdleCameraRotator : MonoBehaviour
{
    
    public float rotatationSpeed = 3;
    public Transform target;
    
    // Update is called once per frame
    void Update()
    {
    transform.RotateAround(target.position, Vector3.up, rotatationSpeed * Time.deltaTime);    
    }
}
