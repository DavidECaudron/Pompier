using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TruckMotor : MonoBehaviour
{
    public float Speed;
    public float Steer { get; set; }
    public float Throttle { get; set; }


    private Rigidbody _rigidbody;
    private Wheel[] _wheels;

    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private float _motorTorque = 1500f;
    [SerializeField] private float _maxSteer = 20f;
    [SerializeField] private float _currentSpeed;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
        _wheels = GetComponentsInChildren<Wheel>();
    }

    void FixedUpdate()
    {
        ApplySpeedOnWheels();
        CalculateSpeed();
    }

    private void ApplySpeedOnWheels()
    {
        foreach (var wheel in _wheels)
        {
            wheel.SteerAngle = Steer * _maxSteer;
            wheel.Torque = Throttle * _motorTorque;
        }
    }

    private void CalculateSpeed()
    {
        _currentSpeed = _rigidbody.velocity.magnitude * 3.6f;
        Speed = Mathf.RoundToInt(_currentSpeed);
    }
}
