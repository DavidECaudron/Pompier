using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool IsInInteraction = false;

    public void PickupObject(Transform parent)
    {
        if (IsInInteraction) return;
        IsInInteraction = true;

        gameObject.transform.SetParent(parent);
    }

    public void DropObject()
    {
        if (!IsInInteraction) return;
        IsInInteraction = false;

        gameObject.transform.SetParent(null);
    }
}
