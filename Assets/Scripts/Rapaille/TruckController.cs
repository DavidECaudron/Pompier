using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float maxSteer = 20f;

    [SerializeField] private float currentSpeed;//ExhaultSmokeFx
    [SerializeField] private float speed;//ExhaultSmokeFx


    public float Steer {get; set; }
    public float Throttle {get; set; }
    private Rigidbody _rigidbody;
    private Wheel[] wheels;

    public ParticleSystem exhaultSmokeFXLeft; //ExhaultSmokeFx
    public ParticleSystem exhaultSmokeFXRight; //ExhaultSmokeFx


    void Start() 
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition; 
    }

    void Update()
    {
        EmissionParticules();
        foreach (var wheel in wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }
    void FixedUpdate()//ExhaultSmokeFx
    {
        currentSpeed = _rigidbody.velocity.magnitude * 3.6f;//ExhaultSmokeFx

        speed = Mathf.RoundToInt(currentSpeed);//ExhaultSmokeFx
    }
    private void EmissionParticules()//ExhaultSmokeFx
    {
        if (speed >5)//ExhaultSmokeFx
        {
            exhaultSmokeFXLeft.Emit(1);//ExhaultSmokeFx
            exhaultSmokeFXRight.Emit(1);//ExhaultSmokeFx
        }
        else//ExhaultSmokeFx
        {
            exhaultSmokeFXLeft.Emit(0);//ExhaultSmokeFx
            exhaultSmokeFXRight.Emit(0);//ExhaultSmokeFx
        }
    }
}
