using System;
using UnityEngine;

public class ItemFixable : ItemInteractable
{
    public PlayerInventory inventory;
    public GameObject brokenItem;
    public GameObject fixedItem;
    
    public int fixDifficulty = 10;
    
    [SerializeField]
    private int fixProgress = 0;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer == null) throw new Exception("The object should have a MeshRenderer Component");
        _meshFilter = GetComponent<MeshFilter>();
        if (_meshFilter == null) throw new Exception("The object should have a MeshFilter Component");
    }

    private void Start()
    {
        _meshFilter.mesh = brokenItem.gameObject.GetComponent<MeshFilter>().mesh;
        _meshRenderer.material = brokenItem.gameObject.GetComponent<MeshRenderer>().material;
    }

    public override void InteractPrimary()
    {
        if (!isBroken()) return;
        

        var item = inventory.GetSelectedItem();
            
        if (item is not ToolFix fix) return;

        var force = fix.fixForce;
        fixProgress += force;
        if (!isBroken())
        {
            _meshFilter.mesh = fixedItem.GetComponent<MeshFilter>().mesh;
            _meshRenderer.material = fixedItem.GetComponent<MeshRenderer>().material;
        }
    }

    public override void InteractSecondary()
    {
        throw new System.NotImplementedException();
    }

    public override string GetInteractionText()
    {
        if (fixProgress <= 0) return "Repair (E)";
        if (isBroken()) return FixPercent() + "% Repaired";
        return "Repaired";
    }

    public bool isBroken()
    {
        return fixProgress < fixDifficulty;
    }

    public float FixPercent()
    {
        return 100f * fixProgress / fixDifficulty;
    }
}