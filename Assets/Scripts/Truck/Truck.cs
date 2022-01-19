using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public List<Player> PlayersInside = new List<Player>();
    [SerializeField] private List<Transform> _playerPosInTruck = new List<Transform>();
    [SerializeField] private Transform _playerRespawn;

    public bool HavePlayerInside
    {
        get { return PlayersInside.Count > 0; }
    }


    public void EnterExitTruck(Player player)
    {
        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();

        if (playerController.IsInTruck)
        {
            PlayerExitTruck(player, playerController);
        }
        else
        {
            PlayerEnterTruck(player, playerController);
        }
    }

    private void PlayerEnterTruck(Player player, PlayerController playerController)
    {
        PlayersInside.Add(player);
        
        Transform transformParent = _playerPosInTruck[PlayersInside.Count - 1];

        playerController.CanMove = false;
        playerController.IsInTruck = true;
        playerController.TruckTransform = transformParent;
        player.transform.SetParent(transformParent);
        player.gameObject.transform.localPosition = Vector3.zero;
        player.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        Debug.Log("Entrer dans le camion");
    }

    private void PlayerExitTruck(Player player, PlayerController playerController)
    {
        PlayersInside.Remove(player);

        playerController.CanMove = true;
        playerController.IsInTruck = false;
        playerController.TruckTransform = null;
        player.transform.SetParent(null);

        player.gameObject.transform.position = _playerRespawn.position;

        playerController.StopPlayerMovement();
    }
}
