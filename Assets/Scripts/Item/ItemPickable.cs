using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemPickable : ItemInteractable
{
    public PlayerInventory inventory;
    
    public void Start()
    {
        InteractionKeyCode = KeyCode.E;
    }
    
    public override void Interact()
    {
        inventory.AddItem(this);
    }

    public override string GetInteractionText()
    {
        if (inventory.isFull())
        {
            return "Press (" + InteractionKeyCode + ") to pick";
        }
        return "Inventory full";
    }

    protected void SetProperties(string iName, float weight)
    {
        this.itemName = iName;
        this.itemWeight = weight;
    }
}