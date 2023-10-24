using System;
using UnityEngine;

public class ItemFixable : ItemInteractable
{
    public PlayerInventory inventory;
    public GameObject fixedItemPrefab;

    private MeshCollider _itemMeshCollider;
    private int fixDifficulty = 10;
    private int fixProgress = 0;

    private void Awake()
    {
        _itemMeshCollider = GetComponent<MeshCollider>();
        if (_itemMeshCollider == null) throw new Exception("The object should have a MeshCollider Component");
    }

    private void Start()
    {
        if (IsBroken())
        {
            GameObject brokenItemInstance = Instantiate(fixedItemPrefab, transform.position, transform.rotation);
            MeshFilter brokenMeshFilter = brokenItemInstance.GetComponent<MeshFilter>();
            UpdateMeshAndCollider(brokenMeshFilter.sharedMesh);
        }
    }

    private void UpdateMeshAndCollider(Mesh mesh)
    {
        _itemMeshCollider.sharedMesh = mesh;
    }

    public override void InteractPrimary()
    {
        if (!IsBroken()) return;

        var item = inventory.GetSelectedItem();

        if (item is not ToolFix fix) return;

        var force = fix.fixForce;
        fixProgress += force;

        if (!IsBroken())
        {
            RepairObject();
        }
    }

    public override void InteractSecondary()
    {
        throw new System.NotImplementedException();
    }

    public override string GetInteractionText()
    {
        if (fixProgress <= 0) return "Repair " + itemName + " (E)";
        if (IsBroken()) return FixPercent() + "% Repaired";
        return itemName;
    }

    public bool IsBroken()
    {
        return fixProgress < fixDifficulty;
    }

    public bool IsFixed()
    {
        return !IsBroken();
    }

    private float FixPercent()
    {
        return 100f * fixProgress / fixDifficulty;
    }

    private void RepairObject()
    {
        Destroy(gameObject); // Destroy the broken object
        Instantiate(fixedItemPrefab, transform.position, transform.rotation); // Instantiate the fixed object
    }
}