using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public Transform centerOfMass;
    public float motorTorque = 1500f;
    public float maxSteer = 20f;
    public float Steer {get; set; }
    public float Throttle {get; set; }
    private Rigidbody _rigidbody;
    private Wheel[] wheels;
    //public ParticleSystem systemParticule;//Smoke
    //public Vector3 startPosition;//Reset
    //public Quaternion startRotation;//Reset

    void Start() 
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;    
    }
     /*void Awake()
    {
        startPosition = transform.position;//Reset
        startRotation = transform.rotation;//Reset
        print(startPosition);//Reset
        print(startRotation);//Reset
    }*/
    /*void FixedUpdate()//Reset
    {
        if (Input.GetKey("r"))//Reset
        {
            transform.position = startPosition;//Reset
            transform.rotation = startRotation;//Reset
            _rigidbody.velocity = new Vector3(0,0,0);//Reset
            _rigidbody.angularVelocity = new Vector3(0,0,0);//Reset
        }
    }*/
    void Update()
    {
        /*if (Input.GetKey(KeyCode.UpArrow))//Smoke
        {
            systemParticule.Emit(1);//Smoke
        }
        else//Smoke
        {
            systemParticule.Emit(0);//Smoke
        }*/


        foreach (var wheel in wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }
}
