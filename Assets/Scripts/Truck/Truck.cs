using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    [HideInInspector]public List<Player> PlayersInside = new List<Player>();
    [HideInInspector] public bool playerInLadderController = false;
    [SerializeField] private List<Transform> _playerPosInTruck = new List<Transform>();
    [SerializeField] private Transform _playerRespawn;
    [SerializeField] private Transform _ladderControlPosition;

    private bool inFireZone = false;

    public bool HavePlayerInside
    {
        get { return PlayersInside.Count > 0; }
    }


    public void EnterExitTruck(Player player)
    {
        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();

        if (playerController.IsInTruck)
        {
            if (GameManager.Instance.truckInFireInstance())
            {
                PlayerExitTruck(player, playerController);
            }
            else
            {
                Debug.Log("YOU NOT ARE IN FIRE ZONE");
            }
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



    public void EnterExitLadderControl(Player player)
    {
        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();

        if (!playerController.IsInTruck)
        {
            if (playerInLadderController)
            {
                Debug.Log("Only 1 player can control ladder");
                return;
            }
            EnterLadderControl(player, playerController);
        }
        else
        {
            ExitLadderControl(player, playerController);
        }
    }

    private void EnterLadderControl(Player player, PlayerController playerController)
    {
        playerInLadderController = true;

        playerController.CanMove = false;
        playerController.IsInTruck = true;
        playerController.TruckTransform = _ladderControlPosition;
        player.transform.SetParent(_ladderControlPosition);
        player.gameObject.transform.localPosition = Vector3.zero;
        player.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void ExitLadderControl(Player player, PlayerController playerController)
    {
        playerInLadderController = false;

        playerController.CanMove = true;
        playerController.IsInTruck = false;
        playerController.TruckTransform = null;
        player.transform.SetParent(null);

        playerController.StopPlayerMovement();
    }
}
