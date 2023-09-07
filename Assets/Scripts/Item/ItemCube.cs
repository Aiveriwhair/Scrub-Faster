using System;
using Unity.VisualScripting;

public class ItemCube : ItemPickable
{
    public new void Start()
    {
        base.Start();
        SetProperties("Some Cube", 2);
    }
}