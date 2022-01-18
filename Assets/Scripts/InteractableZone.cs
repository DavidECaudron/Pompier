using UnityEngine;
using UnityEngine.Events;

public class InteractableZone : MonoBehaviour
{
    [SerializeField] private UnityEvent<Player> _functionToCall;

    public void UseInteractZone(GameObject obj)
    {
        Player player = obj.GetComponent<Player>();
        if (player == null) return;
        Debug.Log("Call interact function");
        _functionToCall?.Invoke(player);
    }
}
