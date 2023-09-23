using System;
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
        if (_dirtManager.IsClean())
        {
            Destroy(this); 
            return;
        }

        if (pointer.PointingAt() is not ItemCleanable) return;
        
        var hit = pointer.GetHit();
        var meshCollider = hit.collider as MeshCollider;
        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            _dirtManager.Clean(hit.point, cleaningTool.radius);
        }
        if(_dirtManager.IsClean()) Destroy(this);
    }
    public override void InteractSecondary()
    {
        throw new System.NotImplementedException();
    }
    public override string GetInteractionText()
    {
        return "Clean (E)";
    }
}
