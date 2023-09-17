public abstract class Interaction
{
    public string InteractionText;
    public int Priority;

    public abstract void ApplyEffectMain();
    public abstract void ApplyEffectOther();
}