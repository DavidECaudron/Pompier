using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private bool steer;
    [SerializeField] private bool invertSteer;
    [SerializeField] private bool power;
    [SerializeField] private WheelCollider wheelCollider;

    public float SteerAngle { get; set; } 
    public float Torque { get; set; }
    
    private Transform wheelTransform;

    void Start()
    {
       wheelCollider = GetComponentInChildren<WheelCollider>(); 
       wheelTransform = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
    }

    void Update()
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    void FixedUpdate() 
    {
        if (steer)
        {
            wheelCollider.steerAngle = SteerAngle * (invertSteer ? -1 : 1);
        }
        if (power)
        {
            wheelCollider.motorTorque = Torque;
        }
    }
}
