using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Range(1, 10)]
    public int inventorySize = 3;
    
    private bool _hasBeenModified = false;
    [SerializeField]
    private List<ItemInteractable> _inventoryItems = new();
    private int _cursor = 0;
    
    public bool Add(ItemInteractable item)
    {
        if (IsFull())
        {
            return false;
        }
        _inventoryItems.Add(item);
        _hasBeenModified = true;
        return true;
    }

    public ItemInteractable GetIndex(int index)
    {
        if (index >= _inventoryItems.Count || index < 0)
        {
            return null;
        }
        return _inventoryItems[index];
    }

    public void Remove(ItemInteractable item)
    {
        if (!_inventoryItems.Remove(item))
        {
            return;
        }
        _hasBeenModified = true;
    }

    public ItemInteractable RemoveAt(int index = 0)
    {
        if (IsEmpty())
        {
            return null;
        }
        var item = _inventoryItems[index];
        _inventoryItems.RemoveAt(index);
        return item;
    }

    public bool Includes(ItemInteractable item)
    {
        return _inventoryItems.Contains(item);
    }

    public void Clear()
    {
        if (IsEmpty())
        {
            return;
        }
        _inventoryItems.Clear();
        _hasBeenModified = true;
    }

    public int Count()
    {
        return _inventoryItems.Count;
    }

    public bool IsFull()
    {
        return Count() >= inventorySize;
    }

    public bool IsEmpty()
    {
        return Count() <= 0;
    }

    public ItemInteractable GetSelectedItem()
    {
        return _inventoryItems[_cursor];
    }

    public bool HasBeenModified()
    {
        return _hasBeenModified;
    }
}