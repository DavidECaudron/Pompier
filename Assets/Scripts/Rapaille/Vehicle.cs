using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public enum ControlType { HumanInput, AI }
    public ControlType controlType = ControlType.HumanInput;
    private TruckController truckController;

    void Awake()
    {
        truckController = GetComponent<TruckController>();
    }
       
    void Update()
    {
        if (controlType == ControlType.HumanInput)
        {
            truckController.Steer = GameManager.Instance.InputController.SteerInput;
            truckController.Throttle = GameManager.Instance.InputController.ThrottleInput;
        } 
    }
}
