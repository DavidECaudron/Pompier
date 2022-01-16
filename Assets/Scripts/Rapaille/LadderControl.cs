using UnityEngine;

public class LadderControl : MonoBehaviour
{
    [SerializeField] private Transform ladder001;
    [SerializeField] private Transform ladder002;
    [SerializeField] private Transform ladder003;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad4))
        {
            gameObject.transform.Rotate(new Vector3(0,1,0),-0.1f);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            gameObject.transform.Rotate(new Vector3(0,1,0),0.1f);
        }
        if (Input.GetKey(KeyCode.Keypad8) && (Vector3.Distance(ladder002.transform.position,gameObject.transform.position) < 5.5f))
        {
            ladder002.transform.Translate(new Vector3(0,0,0.01f),Space.Self);
        }
        if (Input.GetKey(KeyCode.Keypad2) && (Vector3.Distance(ladder002.transform.position,gameObject.transform.position) > 0.01f))
        {
            ladder002.transform.Translate(new Vector3(0,0,-0.01f),Space.Self);
        }
        if (Input.GetKey(KeyCode.Keypad8) && (Vector3.Distance(ladder003.transform.position,gameObject.transform.position) < 11))
        {
            ladder003.transform.Translate(new Vector3(0,0,0.01f),Space.Self);
        }
        if (Input.GetKey(KeyCode.Keypad2) && (Vector3.Distance(ladder003.transform.position,gameObject.transform.position) > 0.01f))
        {
            ladder003.transform.Translate(new Vector3(0,0,-0.01f),Space.Self);
        }
        if (Input.GetKey(KeyCode.KeypadMinus) && (ladder001.transform.rotation.x < 0))
        {
            ladder001.transform.Rotate(new Vector3(1,0,0),0.1f);
        }
        if (Input.GetKey(KeyCode.KeypadPlus) && (ladder001.transform.rotation.x > -80))
        {
            ladder001.transform.Rotate(new Vector3(1,0,0),-0.1f);
        }
        //var rotationX = Mathf.Clamp(transform.eulerAngles.x, -90.0F, 0.0F);
        //transform.rotation = Quaternion.Euler(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        
    }
}
