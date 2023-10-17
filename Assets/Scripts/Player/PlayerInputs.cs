using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private PlayerPointer _pointer;
    private PlayerInventory _inventory;
    
    [Header("Input keys")]
    public KeyCode dropKeyCode = KeyCode.R;
    public KeyCode interactPrimaryKeyCode = KeyCode.E;
    public KeyCode interactSecondaryKeyCode = KeyCode.Q;
    public void Start()
    {
        _inventory = GetComponent<PlayerInventory>();
        _pointer = GetComponent<PlayerPointer>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(dropKeyCode))
        {
            var item = _inventory.RemoveAt(0);
            if (item)
            {
                ((ItemPickable)item).Drop();
            }
        }
        if (Input.GetKeyDown(interactPrimaryKeyCode))
        {
            var inRangeItem = _pointer.PointingAt();
            var selectedItem = _inventory.GetSelectedItem();
            if (inRangeItem)
            {
                inRangeItem.InteractPrimary();
            }
            else if (selectedItem is Tool)
            {
                selectedItem.InteractPrimary();
            }
        }
        if (Input.GetKeyDown(interactSecondaryKeyCode))
        {
            var inRangeItem = _pointer.PointingAt();
            if (inRangeItem)
            {
                inRangeItem.InteractSecondary();
            }
        }
    }
}