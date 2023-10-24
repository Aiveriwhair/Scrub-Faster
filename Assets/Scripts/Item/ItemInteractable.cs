using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemInteractable : MonoBehaviour
{
    public string itemName = "Unset Name";
    public int itemMass = 1;
    [HideInInspector]
    public bool isGlowing = false;
    
    
    public void Update()
    {
        GetComponent<Outline>().enabled = isGlowing;
        isGlowing = false;
    }

    public abstract void InteractPrimary();
    public abstract void InteractSecondary();
    public abstract string GetInteractionText();
}
