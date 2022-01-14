using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float maxSteer = 20f;

    public float Steer {get; set; }
    public float Throttle {get; set; }
    private Rigidbody _rigidbody;
    private Wheel[] wheels;

    void Start() 
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;    
    }
    
    void Update()
    {
        foreach (var wheel in wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }
}
