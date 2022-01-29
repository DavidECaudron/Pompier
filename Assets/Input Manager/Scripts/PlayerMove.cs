using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _movementForce = 1.0f;
    [SerializeField] private float _maxSpeed = 5.0f;
    [SerializeField] private Camera _camera;

    private Vector3 _forceDirection = new Vector3(0.0f, 0.0f, 0.0f);
    private Rigidbody _rigidBody;

    private ActionAsset _actionsAsset;
    private InputAction _inputAction;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _actionsAsset = new ActionAsset();
    }
    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }
    private void OnEnable()
    {
        _inputAction = _actionsAsset.Player.Move;
        _actionsAsset.Player.Move.Enable();
    }
    private void OnDisable()
    {
        _actionsAsset.Player.Move.Disable();
    }

    private void Movement()
    {
        _forceDirection += _inputAction.ReadValue<Vector2>().x * GetCameraRight(_camera) * _movementForce;
        _forceDirection += _inputAction.ReadValue<Vector2>().y * GetCameraForward(_camera) * _movementForce;

        _rigidBody.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = new Vector3(0.0f, 0.0f, 0.0f);

        if (_rigidBody.velocity.y < 0.0f)
        {
            _rigidBody.velocity -= new Vector3(0.0f, -1.0f, 0.0f) * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = _rigidBody.velocity;
        horizontalVelocity.y = 0.0f;
        if (horizontalVelocity.sqrMagnitude > _maxSpeed * _maxSpeed)
        {
            _rigidBody.velocity = horizontalVelocity.normalized * _maxSpeed + new Vector3(0.0f, 1.0f, 0.0f) * _rigidBody.velocity.y;
        }
    }
    private Vector3 GetCameraForward(Camera _camera)
    {
        Vector3 forward = _camera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    private Vector3 GetCameraRight(Camera _camera)
    {
        Vector3 right = _camera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    private void Rotation()
    {
        Vector3 direction = _rigidBody.velocity;
        direction.y = 0f;

        if (_inputAction.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            _rigidBody.rotation = Quaternion.LookRotation(direction, new Vector3(0.0f, 1.0f, 0.0f));
        }
        else
        {
            _rigidBody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
