using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public List<Player> PlayersInside = new List<Player>();
    [SerializeField] private List<Transform> _playerPosInTruck = new List<Transform>();

    public bool HavePlayerInside
    {
        get { return PlayersInside.Count > 0; }
    }

    public void PlayerEnterTruck(Player player)
    {
        //Placer joueur enfant du camion
        //placer le joueur dans la camion
        //Desactiver deplacement du joueur
        PlayersInside.Add(player);

        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
        Transform transformParent = _playerPosInTruck[PlayersInside.Count - 1];

        playerController.CanMove = false;
        playerController.IsInTruck = true;
        playerController.TruckTransform = transformParent;
        player.transform.SetParent(transformParent);
        player.gameObject.transform.localPosition = Vector3.zero;
        player.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        Debug.Log("Entrer dans le camion");
    }

    public void PlayerExitTruck(Player player)
    {
        PlayersInside.Remove(player);

        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
        playerController.CanMove = true;
        playerController.IsInTruck = false;
        playerController.TruckTransform = null;

        player.transform.SetParent(null);
    }
}
