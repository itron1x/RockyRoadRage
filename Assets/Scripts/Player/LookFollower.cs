using UnityEngine;

public class LookFollower : MonoBehaviour
{
    
    public Transform Target;
    public float TargetHeight;
    public Transform Camera;
    
    void LateUpdate()
    {
        transform.position = Target.position + new Vector3(0, TargetHeight, 0);
        Vector3 direction = transform.position - Camera.position;
        direction = - direction;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
