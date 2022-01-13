using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private GameObject player;
    private bool isInRange = false;
    private bool isInInteraction = false;

    void Update()
    {
        PerformInteraction();
    }

    private void PerformInteraction()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isInInteraction)
            {
                gameObject.transform.SetParent(player.transform);
                isInInteraction = true;
            }
            if (Input.GetKeyDown(KeyCode.A) && isInInteraction)
            {
                gameObject.transform.SetParent(null);
                isInInteraction = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            player = other.gameObject;
            Debug.Log("isInRange");
            isInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("isOffRange");
            isInRange = false;
        }
    }
}
