using Player_2._0;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundDetection2 groundDetection;
    
    [Header("Movement attributes")] 
    [SerializeField] private int speed;
    [SerializeField] private int acceleration;
    [SerializeField] private int jumpHeight;
    [SerializeField] private float mass;

    public Rigidbody GetRigidbody(){
        return rb;
    }

    public GroundDetection2 GetGroundDetection(){
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
