using UnityEngine;

public class LadderControl : MonoBehaviour
{
    [SerializeField] private Transform ladder001;
    [SerializeField] private Transform ladder002;
    [SerializeField] private Transform ladder003;

    [SerializeField] private float _minAngle = 300f;
    [SerializeField] private float _maxAngle = 359f;

    void Start()
    {
        //Start l'echelle à l'angle 359
        ladder001.transform.rotation = Quaternion.Euler(new Vector3(359f, 0f, 0f));
    }
    void Update()
    {
        RightLeftLadder();
        ExtandRetractLadder();
        UpDownLadder();
    }
    private void RightLeftLadder()
    {
        if (Input.GetKey(KeyCode.Keypad4))
        {
            gameObject.transform.Rotate(new Vector3(0, 1, 0), -0.1f);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            gameObject.transform.Rotate(new Vector3(0, 1, 0), 0.1f);
        }
    }
    private void ExtandRetractLadder()
    {
        if (Input.GetKey(KeyCode.Keypad8) && (Vector3.Distance(ladder002.transform.position, gameObject.transform.position) < 5.5f))
        {
            ladder002.transform.Translate(new Vector3(0, 0, 0.005f), Space.Self);
            if (Vector3.Distance(ladder003.transform.position, gameObject.transform.position) < 11)
            {
                ladder003.transform.Translate(new Vector3(0, 0, 0.005f), Space.Self);
            }
        }

        if (Input.GetKey(KeyCode.Keypad2) && (Vector3.Distance(ladder002.transform.position, gameObject.transform.position) > 0.01f))
        {
            ladder002.transform.Translate(new Vector3(0, 0, -0.005f), Space.Self);
            if (Vector3.Distance(ladder003.transform.position, gameObject.transform.position) > 0.01f)
            {
                ladder003.transform.Translate(new Vector3(0, 0, -0.005f), Space.Self);
            }
        }

    }
    private void UpDownLadder()
    {
        Vector3 rotation = ladder001.transform.rotation.eulerAngles;
        float actualAngle = rotation.x;

        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            actualAngle += .1f;
        }
        else if (Input.GetKey(KeyCode.KeypadPlus))
        {
            actualAngle -= .1f;
        }

        actualAngle = Mathf.Clamp(actualAngle, _minAngle, _maxAngle);

        //On ne bouge que si il y a eu une différence d'angle
        if (actualAngle != rotation.x)
        {
            ladder001.transform.rotation = Quaternion.Euler(new Vector3(actualAngle, rotation.y, rotation.z));
        }
    }
}
