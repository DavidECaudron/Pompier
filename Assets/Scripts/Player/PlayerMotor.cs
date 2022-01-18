using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _rotation;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        PerformVelocity();
        PerformRotation();
    }

    public void SetVelocity(Vector3 velocity)
    {
        this._velocity = velocity;
    }

    public void SetRotation(Vector3 rotation)
    {
        this._rotation = rotation;
    }

    private void PerformVelocity()
    {
        if (_velocity != Vector3.zero)
        {
            _rigidBody.MovePosition(_rigidBody.position + _velocity * Time.fixedDeltaTime);
        }
    }

    private void PerformRotation()
    {
        _rigidBody.MoveRotation(_rigidBody.rotation * Quaternion.Euler(_rotation));
    }
}
