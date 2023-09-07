using UnityEngine;

public abstract class ItemInteractable : MonoBehaviour
{
    public string itemName = "Unset Name";
    public float itemWeight = .1f;
    protected KeyCode InteractionKeyCode { get; set; }
    
    public abstract void Interact();
    public abstract string GetInteractionText();
}
