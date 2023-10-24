using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Range(1, 10)]
    public int inventorySize = 3;
    [SerializeField] private ItemPickable[] inventoryItems;
    private int _itemsNumber = 0;
    private int _cursor = 0;
    
    // 
    private PlayerItemCarry _itemCarry;
    
    // UI
    private List<Image> _inventoryImages = new();
    public Sprite defaultIcon;
    public Sprite emptyIcon;
    public Image toolbar;

    private void Awake()
    {
        _itemCarry = GetComponent<PlayerItemCarry>();
        if (_itemCarry == null) throw new MissingComponentException("GameObject must have a PlayerItemCarryComponent");
    }


    public void Start()
    {
        inventoryItems = new ItemPickable[inventorySize];
        
        // LOAD UI
        foreach (Transform child in toolbar.transform)
        {
            var childGameObject = child.gameObject.GetComponent<Image>();
            if(childGameObject) _inventoryImages.Add(childGameObject);
        }
        UpdateInventoryUI();
    }


    private void UpdateInventoryUI()
    {
        if (_inventoryImages.Count < inventorySize)
        {
            print("UI cannot display the whole inventory");
            return;
        }
        
        for (var i = 0; i < inventorySize; i++)
        {
            var currentItem = GetIndex(i);
            if (currentItem is null)
            {
                _inventoryImages[i].sprite = emptyIcon;
            }
            else if (currentItem.itemIcon is null)
            {
                _inventoryImages[i].sprite = defaultIcon;
            }
            else
            {
                _inventoryImages[i].sprite = currentItem.itemIcon;
            }
        }
    }
    
    public bool Add(ItemPickable item)
    {
        if (IsFull())
        {
            return false;
        }
        inventoryItems[_itemsNumber] = item;
        _itemsNumber++;
        UpdateInventoryUI();
        item.gameObject.SetActive(false);
        _itemCarry.ManualUpdate();
        return true;
    }

    public ItemPickable GetIndex(int index)
    {
        if (index >= inventorySize || index < 0)
        {
            return null;
        }
        return inventoryItems[index];
    }

    private ItemPickable RemoveAt(int index)
    {
        if (IsEmpty())
        {
            return null;
        }
        var item = inventoryItems[index];
        inventoryItems[index] = null;
        if (item)
        {
            _itemsNumber--;
            UpdateInventoryUI();
            _itemCarry.ManualUpdate();
        }
        return item;
    }

    public ItemPickable RemoveAtCursor()
    {
        var item = RemoveAt(_cursor);
        return item;
    }

    public int Count()
    {
        return _itemsNumber;
    }

    public bool IsFull()
    {
        return Count() >= inventorySize;
    }

    public bool IsEmpty()
    {
        return Count() <= 0;
    }

    public ItemPickable GetSelectedItem()
    {
        return inventoryItems[_cursor];
    }

    public bool Contains(ItemPickable item)
    {
        return inventoryItems.Contains(item);
    }
    
    public void CursorAt(int index)
    {
        if (index >= inventorySize || index == _cursor) return;
        _cursor = index;
        _itemCarry.ManualUpdate();
    }
}