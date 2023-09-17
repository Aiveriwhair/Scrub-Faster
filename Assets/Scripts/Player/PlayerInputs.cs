using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private PlayerPointer _pointer;
    private PlayerInventory _inventory;
    
    [Header("Input keys")]
    public KeyCode dropKeyCode = KeyCode.A;
    public KeyCode interactKeyCode = KeyCode.E;
    public KeyCode openMenuKeyCode = KeyCode.Escape;
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
        if (Input.GetKeyDown(interactKeyCode))
        {
            var inRangeItem = _pointer.PointingAt();
            if (inRangeItem)
            {
                inRangeItem.Interact();
            }
        }
    }
}