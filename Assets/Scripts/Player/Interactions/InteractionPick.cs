using UnityEditor;

public class InteractionCanPick : Interaction
{
    private ItemPickable _item;
    public InteractionCanPick(ItemPickable item)
    {
        _item = item;
        this.Priority = 1;
        this.InteractionText = "Press (e) to pick";
    }
    public override void ApplyEffectMain()
    {
    }

    public override void ApplyEffectOther()
    {
        
    }
}