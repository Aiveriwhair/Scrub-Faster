using System;
using UnityEngine;

public abstract class ItemInteractable : MonoBehaviour
{
    public string itemName = "Unset Name";
    public float itemWeight = .1f;
    protected KeyCode InteractionKeyCode { get; set; }

    public void MakeGlow()
    {
        this.GetComponent<Outline>().enabled = true;
    }

    public void Update()
    {
        this.GetComponent<Outline>().enabled = false;
    }

    public abstract void Interact();
    public abstract string GetInteractionText();
}
