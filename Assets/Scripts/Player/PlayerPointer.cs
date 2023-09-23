using UnityEngine;
using UnityEngine.UI;
public class PlayerPointer : MonoBehaviour
{
    public Text pointerInfoDisplay;
    public float interactDistance = 3f;
    public Transform orientation;
    public LayerMask detectedLayer;
    
    private ItemInteractable _pointedItem = null;
    private RaycastHit _hit;


    public ItemInteractable PointingAt()
    {
        return !_pointedItem ? null : _pointedItem;
    }

    public RaycastHit GetHit()
    {
        return _hit;
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
                pointerInfoDisplay.gameObject.SetActive(false);
            }
        }
        else
        {
            pointerInfoDisplay.gameObject.SetActive(false);
        }
    }
}
