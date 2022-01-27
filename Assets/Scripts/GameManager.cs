using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static CityFireManager InstanceCityFireManager { get; private set; }
    public static readonly float MAX_DISTANCE_PLAYER_TRUC = 100;

    [SerializeField] private GameObject spawnTruck;
    [SerializeField] private GameObject truckPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject city;

    private GameObject truck = null;
    private List<GameObject> players = new List<GameObject>();

    void Awake()
    {        
        if(Instance != null)
        {
            Debug.LogError("Instance of GameManager already exist");
            Destroy(gameObject);
        }

        Instance = this;
        CityFireManager.Instance = GetComponent<CityFireManager>();
    }

    private void Start()
    {
        Debug.Log("Awake Start");
        if(truck == null)
        {
            truck = Instantiate(truckPrefab, spawnTruck.transform.position, Quaternion.Euler(0,0,0));
        }

        var pos = truck.transform.position;
        
        pos.y = pos.y + 5;

        players.Add(Instantiate(playerPrefab, pos, Quaternion.Euler(0,0,0)));
        
        players[0].GetComponent<PlayerController>().truck = truck;

        CityFireManager.Instance.StartFire();
    }

    private void Update()
    {
        if(truck.transform.position.y < -10)
        {
            truck.transform.position = new Vector3(100, 10f, 100);
        }

        foreach (var player in players)
        {
            if(Vector3.Distance(player.transform.position, truck.transform.position) > MAX_DISTANCE_PLAYER_TRUC)
            {
                var pos = truck.transform.position;
                pos.y = pos.y + 10;
                player.transform.position = pos;
            }
        }
    }

    public bool truckInFireInstance()
    {
        return false;
    }
}
