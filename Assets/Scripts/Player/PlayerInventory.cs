using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemInteractable> _inventoryItems = new();
    
    public int inventorySize = 3;
    public int cursor = 0; 
    
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

    public ItemInteractable GetIndex(int index)
    {
        if (index >= _inventoryItems.Count || index < 0)
        {
            return null;
        }
        else
        {
            return _inventoryItems[index];
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

    public int Count()
    {
        return _inventoryItems.Count;
    }
    
    public bool IsFull()
    {
        return _inventoryItems.Count < inventorySize;
    }
}