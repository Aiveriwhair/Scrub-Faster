public class ItemCube : ItemPickable
{
    public ItemCube(string name, float weight, PlayerInventory inventory) : base(name,weight,inventory) {}
    
    public override void Interact()
    {
        inventory.AddItem(this);
    }
}