using UnityEngine;

[RequireComponent(typeof(TruckMotor))]
public class TruckController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _exhaultSmokeFXLeft;
    [SerializeField] private ParticleSystem _exhaultSmokeFXRight;

    private TruckMotor _motor;

    void Start()
    {
        _motor = GetComponent<TruckMotor>();
    }

    void Update()
    {
        GetVelocity();
        EmissionParticules();
    }

    private void GetVelocity()
    {
        _motor.Steer = Input.GetAxis("Horizontal");
        _motor.Throttle = Input.GetAxis("Vertical");
        _motor.Throttle = Mathf.Clamp(_motor.Throttle, -.5f, 1);
    }

    private void EmissionParticules()
    {
        if (_motor.Speed > 5)
        {
            _exhaultSmokeFXLeft.Emit(1);
            _exhaultSmokeFXRight.Emit(1);
        }
        else
        {
            _exhaultSmokeFXLeft.Emit(0);
            _exhaultSmokeFXRight.Emit(0);
        }
    }
}
