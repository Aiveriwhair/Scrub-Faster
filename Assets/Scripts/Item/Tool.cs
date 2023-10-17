public class Tool : ItemPickable
{
    public override string GetInteractionText()
    {
        if(!_isPicked)
        {
            return inventory.IsFull() ? "Inventory full" : "Pick " + itemName + " (E)";
        }
        return "Use " + itemName + " (E)";    
    }
    
}