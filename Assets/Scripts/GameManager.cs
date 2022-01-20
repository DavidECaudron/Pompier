using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public GameObject truckPrefab;
    public GameObject truck = null;

    void Awake()
    {        
        if(Instance != null)
        {
            Debug.LogError("Instance of GameManager already exist");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        if(truck == null)
        {
            truck = Instantiate(truckPrefab, new Vector3(100,10,100), Quaternion.Euler(0,0,0));
        }
    }

    private void Update()
    {
        if(truck.transform.position.y < -10)
        {
            truck.transform.position = new Vector3(100, 10f, 100);
        }
    }
}
