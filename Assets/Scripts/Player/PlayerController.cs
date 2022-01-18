using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    private PlayerMotor playerMotor;

    private void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        GetVelocity();
        GetRotation();
    }

    private void GetVelocity()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * horizontal;
        Vector3 moveVertical = transform.forward * vertical;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        playerMotor.SetVelocity(velocity);
    }

    private void GetRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0.0f, mouseX, 0.0f) * rotationSpeed;

        playerMotor.SetRotation(rotation);
    }
}
