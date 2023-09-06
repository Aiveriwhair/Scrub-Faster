using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemInteractable> _inventoryItems = new List<ItemInteractable>();

    public int inventorySize = 3;
    
    public bool AddItem(ItemInteractable item)
    {
        if (_inventoryItems.Count < inventorySize)
        {
            _inventoryItems.Add(item);
            item.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(ItemInteractable item)
    {
        _inventoryItems.Remove(item);
    }

    public bool ContainsItem(ItemInteractable item)
    {
        return _inventoryItems.Contains(item);
    }

    public void ClearInventory()
    {
        _inventoryItems.Clear();
    }

    public bool isFull()
    {
        return _inventoryItems.Count < inventorySize;
    }
}