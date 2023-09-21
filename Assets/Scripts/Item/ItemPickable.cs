using Unity.VisualScripting;
using UnityEngine;

public class ItemPickable : ItemInteractable
{
    [Header("Drop")]
    public Transform handLocation;
    public Transform visionOrientation;
    public float dropForce;
    
    [Header("Pick")]
    public PlayerInventory inventory;
    
    private new void Update()
    {
        GetComponent<Outline>().enabled = isGlowing;
        isGlowing = false;
    }

    public void Drop()
    {
        var handPosition = handLocation.transform;
        var handPositionForward = visionOrientation.forward;
        
        transform.position = handPosition.position + handPositionForward.normalized;
        transform.rotation = handPosition.rotation;
        gameObject.SetActive(true);
        GetComponent<Rigidbody>().AddForce(handPositionForward * dropForce, ForceMode.Impulse);
     
    }
    
    public override void InteractPrimary()
    {
        if(!inventory.Add(this)) return;
        gameObject.SetActive(false);
    }

    public override void InteractSecondary()
    {
        throw new System.NotImplementedException();
    }

    public override string GetInteractionText()
    {
        if (inventory.IsFull())
        {
            return "Inventory full";
        }
        return "Pick (E)";
    }
}