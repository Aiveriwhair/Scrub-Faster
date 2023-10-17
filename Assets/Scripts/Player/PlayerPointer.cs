using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPointer : MonoBehaviour
{
    public Text pointerInfoDisplay;
    public float interactDistance = 3f;
    public Transform orientation;
    public LayerMask detectedLayer;
    public PlayerInventory inventory;
    
    private ItemInteractable _pointedItem = null;
    private RaycastHit _hit;
    private bool _isHitting = false;


    public ItemInteractable PointingAt()
    {
        return !_pointedItem ? null : _pointedItem;
    }

    public RaycastHit GetLastHit()
    {
        return _hit;
    }

    public bool IsHitting()
    {
        return _isHitting;
    }
    
    public bool Raycast(LayerMask detectLayer, out RaycastHit hit)
    {
        return Physics.Raycast(orientation.position, orientation.forward, out hit, interactDistance, detectLayer);
    }
    
    private void Start()
    {
        pointerInfoDisplay.gameObject.SetActive(false);
    }
    private void Update()
    {
        var rayStart = orientation.position;
        var rayDirection = orientation.forward;
        _pointedItem = null;

        if (Physics.Raycast(rayStart, rayDirection, out _hit, interactDistance, detectedLayer))
        {
            _isHitting = true;
            var interactableObject = _hit.collider.GetComponent<ItemInteractable>();
            if (interactableObject != null)
            {
                pointerInfoDisplay.gameObject.SetActive(true);

                pointerInfoDisplay.text = interactableObject.GetInteractionText();
                interactableObject.isGlowing = true;

                _pointedItem = interactableObject;
            }
            else
            {
                var currentTool = inventory.GetSelectedItem();
                if (currentTool is not Tool)
                {
                    pointerInfoDisplay.gameObject.SetActive(false);
                    return;
                }
                pointerInfoDisplay.text = currentTool.GetInteractionText();
            }
        }
        else
        {
            var currentTool = inventory.GetSelectedItem();
            if (currentTool is not Tool)
            {
                pointerInfoDisplay.gameObject.SetActive(false);
                return;
            }
            pointerInfoDisplay.text = currentTool.GetInteractionText();
        }
    }
}
