using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private enum ControlType { HumanInput, AI }
    [SerializeField] private ControlType controlType = ControlType.HumanInput;

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
