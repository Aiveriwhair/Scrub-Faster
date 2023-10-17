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

    protected bool _isPicked = false;
    
    private new void Update()
    {
        GetComponent<Outline>().enabled = isGlowing;
        isGlowing = false;
    }

    public void Drop()
    {
        var handPosition = handLocation.transform;
        var handPositionForward = visionOrientation.forward;
        
        transform.position = handPosition.position;
        transform.rotation = handPosition.rotation;
        gameObject.SetActive(true);

        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(handPositionForward * dropForce, ForceMode.Impulse);
        _isPicked = false;
    }
    
    public override void InteractPrimary()
    {
        if(_isPicked) return;
        if(!inventory.Add(this)) return;
        gameObject.SetActive(false);
        _isPicked = true;
    }

    public override void InteractSecondary()
    {
        throw new System.NotImplementedException();
    }

    public override string GetInteractionText()
    {
        return inventory.IsFull() ? "Inventory full" : "Pick (E)";
    }
}