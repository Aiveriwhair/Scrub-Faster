using Unity.VisualScripting;
using UnityEngine;

public class ItemPickable : ItemInteractable
{
    [Header("Drop")]
    public Transform handLocation;
    public float dropForce;
    
    [Header("Pick")]
    public PlayerInventory inventory;
    
    public void Start()
    {
        InteractionKeyCode = KeyCode.E;
    }

    private new void Update()
    {
        GetComponent<Outline>().enabled = isGlowing;
        isGlowing = false;
    }

    public void Drop()
    {
        var handPosition = handLocation.transform;
        var handPositionForward = handPosition.forward;
        
        transform.position = handPosition.position + handPositionForward.normalized;
        transform.rotation = handPosition.rotation;
        GetComponent<Rigidbody>().AddForce(handPositionForward * dropForce, ForceMode.Impulse);
        gameObject.SetActive(true);
    }
    
    public override void Interact()
    {
        if(!inventory.Add(this)) return;
        gameObject.SetActive(false);
    }

    public override string GetInteractionText()
    {
        if (inventory.IsFull())
        {
            return "Inventory full";
        }
        return "Pick (" + InteractionKeyCode + ")";
    }
}