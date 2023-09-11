using Unity.VisualScripting;
using UnityEngine;

public class ItemPickable : ItemInteractable
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
        if (inventory.IsFull())
        {
            return "Press (" + InteractionKeyCode + ") to pick";
        }
        return "Inventory full";
    }
}