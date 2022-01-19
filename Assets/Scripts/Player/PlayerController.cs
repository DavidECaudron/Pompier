using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _rotationSpeed = 5.0f;

    private PlayerMotor _motor;
    private List<InteractableObject> _objectInRange = new List<InteractableObject>();

    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public bool CanInteract = true;
    [HideInInspector] public bool IsInTruck = false;

    private bool _isInInteraction = false;
    private InteractableZone _interactableZone;
    public Transform TruckTransform;

    private void Start()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        if (CanMove)
        {
            GetVelocity();
            GetRotation();
        }
        else if (!CanMove && IsInTruck)
        {
            gameObject.transform.position = TruckTransform.position;
        }

        InteractWithObject();
    }

    public void StopPlayerMovement()
    {
        _motor.StopMovement();
    }

    private void InteractWithObject()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract)
        {
            if (_interactableZone != null)
            {
                //utiliser interaction de la zone
                Debug.Log("Interact with zone");
                _interactableZone.UseInteractZone(gameObject);

                _motor.SetVelocity(Vector3.zero);
                _motor.SetRotation(Vector3.zero);
            }


            if (!_isInInteraction)
            {
                PickObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    private void PickObject()
    {
        float distance = float.MaxValue;
        InteractableObject objectToInteract = null;

        foreach (InteractableObject obj in _objectInRange)
        {
            if (!obj.IsInInteraction && Vector3.Distance(obj.transform.position, gameObject.transform.position) < distance)
            {
                objectToInteract = obj;
            }
        }

        if (objectToInteract == null) return;
        objectToInteract.GetComponent<InteractableObject>().PickupObject(gameObject.transform);
        _isInInteraction = true;
    }

    private void DropObject()
    {
        GetComponentInChildren<InteractableObject>().DropObject();
        _isInInteraction = false;
    }

    private void GetVelocity()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * horizontal;
        Vector3 moveVertical = transform.forward * vertical;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * _speed;

        _motor.SetVelocity(velocity);
    }

    private void GetRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0.0f, mouseX, 0.0f) * _rotationSpeed;

        _motor.SetRotation(rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.gameObject.GetComponent<InteractableObject>();

        if (interactableObject != null)
        {
            _objectInRange.Add(interactableObject);
        }

        InteractableZone interactableZone = other.gameObject.GetComponent<InteractableZone>();
        if (interactableZone != null)
        {
            _interactableZone = interactableZone;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject interactableObject = other.gameObject.GetComponent<InteractableObject>();

        if (interactableObject != null)
        {
            _objectInRange.Remove(interactableObject);
        }

        if (other.gameObject.GetComponent<InteractableZone>() != null)
        {
            _interactableZone = null;
        }
    }
}
