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

    public override string getInteractionText()
    {
        if (inventory.isFull())
        {
            return "Press (" + InteractionKeyCode + ") to pick";
        }
        return "Inventory full";
    }
}