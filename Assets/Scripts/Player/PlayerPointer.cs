using UnityEngine;
using UnityEngine.UI;
public class PlayerPointer : MonoBehaviour
{
    public Text interactText;
    public float interactDistance = 3f;
    public Transform orientation;

    private void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 rayStart = orientation.position;
        Vector3 rayDirection = orientation.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayStart, rayDirection, out hit, interactDistance))
        {
            var interactableObject = hit.collider.GetComponent<ItemInteractable>();

            if (interactableObject != null)
            {
                interactText.gameObject.SetActive(true);

                interactText.text = interactableObject.GetInteractionText();
                interactableObject.MakeGlow();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactableObject.Interact();
                }
            }
            else
            {
                interactText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }
}
