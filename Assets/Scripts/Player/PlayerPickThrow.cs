using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickThrow : MonoBehaviour
{
    [Header("References")] 
    public Transform camera;
    public Transform attackPoint;
    public GameObject throwableObject;

    [Header("Settings")] 
    public int totalThrows;
    public float throwCooldown;
    public float throwForce;
    public float throwUpwardForce;

    [Header("Keybinds")] 
    public KeyCode throwKey;
    public KeyCode pickKey;

    private bool _readyToThrow;
    private bool _readyToPick;
    
    void Start()
    {
        _readyToThrow = true;
        _readyToPick = true;
    }

    void Update()
    {
        
    }
    
    private void Throw(){}
    private void Pick(){}
}
