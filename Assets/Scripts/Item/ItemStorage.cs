using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : ItemInteractable
{
    public PlayerInventory playerInventory;
    public int maxStorageWeight = 10;
    
    private int _currentWeight;
    private List<ItemPickable> _storedItems = new();
    
    public override void InteractPrimary()
    {
        if (playerInventory.IsEmpty()) return;
        var item = playerInventory.GetSelectedItem();
        if (item is not ItemPickable) return;
        
        if (IsFull()) return;
        if (_currentWeight + item.itemMass >= maxStorageWeight) return;
        
        _currentWeight += item.itemMass;
        _storedItems.Add((ItemPickable)playerInventory.RemoveAt(0));
    }

    public override void InteractSecondary()
    {
        if (IsEmpty()) return;
        
        var item = _storedItems[0];
        if (!playerInventory.Add(item)) return;
        _storedItems.RemoveAt(0);
        _currentWeight -= item.itemMass;
    }

    public override string GetInteractionText()
    {
        if (IsFull())
        {
            return "Storage Full. Take (A)";
        }
        if (!playerInventory.IsEmpty()  && _currentWeight + playerInventory.GetSelectedItem().itemMass >= maxStorageWeight)
        {
            return "Not Enough Space. Take (A)";
        }
        return "Store (E)" + (IsEmpty() ? "" : ". Take (A)");
    }
    
    public bool IsFull()
    {
        return _currentWeight >= maxStorageWeight;
    }

    public bool IsEmpty()
    {
        return Count() == 0;
    }

    public int Count()
    {
        return _storedItems.Count;
    }
    
    public ItemPickable[] GetStoredItems()
    {
        return _storedItems.ToArray();
    }
}
