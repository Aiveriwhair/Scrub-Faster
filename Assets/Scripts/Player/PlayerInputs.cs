using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private ItemDropper _dropper;
    public KeyCode dropKeyCode = KeyCode.A;
    public void Start()
    {
        _dropper = GetComponent<ItemDropper>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(dropKeyCode))
        {
            _dropper.DropSelectedItem();
        }
    }
}