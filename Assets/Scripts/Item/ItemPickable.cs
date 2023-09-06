using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemPickable : ItemInteractable
{
    public PlayerInventory inventory;
    protected ItemPickable(string name, float weight, PlayerInventory inventory)
    {
        this.inventory = inventory;
        itemName = name;
        itemWeight = weight;
        InteractionName = "Pick";
        InteractionKeyCode = KeyCode.E;
    }

    public override string getInteractionText()
    {
        if (inventory.isFull())
        {
            return "Press (" + InteractionKeyCode.ToString() + ") to pick";
        }
        else
        {
            return "Inventory full";
        }
    }
}