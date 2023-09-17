using System;
using UnityEngine;

public abstract class ItemInteractable : MonoBehaviour
{
    public string itemName = "Unset Name";
    public float itemMass = .1f;
    [HideInInspector]
    public bool isGlowing = false;

    protected KeyCode InteractionKeyCode { get; set; }
    
    public void Update()
    {
        GetComponent<Outline>().enabled = isGlowing;
        isGlowing = false;
    }

    public abstract void Interact();
    public abstract string GetInteractionText();
}
