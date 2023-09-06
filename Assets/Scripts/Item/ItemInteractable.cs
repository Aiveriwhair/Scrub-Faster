using UnityEngine;

public abstract class ItemInteractable : MonoBehaviour
{
    public string itemName;
    public float itemWeight;
    protected KeyCode InteractionKeyCode { get; set; }
    public abstract void Interact();
    public abstract string getInteractionText();
}
