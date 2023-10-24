using System;
using Unity.VisualScripting;
using UnityEngine;
public class ItemCleanable : ItemInteractable
{
    private DirtManager _dirtManager;
    public PlayerInventory inventory;
    public PlayerPointer pointer;
    void Start()
    {
        _dirtManager = GetComponent<DirtManager>();
        if (!_dirtManager) throw new Exception("ItemCleanable must carry a DirtManager component.");
    }
    public override void InteractPrimary()
    {
        var cleaningTool = inventory.GetSelectedItem() as ToolClean; 
        if(!cleaningTool) return;
        cleaningTool.InteractPrimary();
        if (_dirtManager.IsClean()) return;
    }
    public override void InteractSecondary()
    {
        throw new System.NotImplementedException();
    }
    public override string GetInteractionText()
    {
        return "Clean (E)";
    }

    public bool isClean()
    {
        return _dirtManager.IsClean();
    }
}
