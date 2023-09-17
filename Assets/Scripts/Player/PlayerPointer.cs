using UnityEngine;
using UnityEngine.UI;
public class PlayerPointer : MonoBehaviour
{
    public Text pointerInfoDisplay;
    public float interactDistance = 3f;
    public Transform orientation;
    public LayerMask detectedLayer;
    private ItemInteractable _pointedItem = null;


    public ItemInteractable PointingAt()
    {
        return !_pointedItem ? null : _pointedItem;
    }
    
    private void Start()
    {
        pointerInfoDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 rayStart = orientation.position;
        Vector3 rayDirection = orientation.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayStart, rayDirection, out hit, interactDistance, detectedLayer))
        {
            var interactableObject = hit.collider.GetComponent<ItemInteractable>();

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
                _pointedItem = null;
            }
        }
        else
        {
            pointerInfoDisplay.gameObject.SetActive(false);
        }
    }
}
