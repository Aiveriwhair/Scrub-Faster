using Unity.VisualScripting;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public Transform dropLocation;
    public float dropForce = 5f;

    private PlayerInventory _playerInventory;
    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
    }

    public void DropSelectedItem()
    {
        if (_playerInventory.Count() > 0)
        {
            var selectedItem = _playerInventory.GetIndex(0);
            selectedItem.transform.position = dropLocation.transform.position + dropLocation.transform.forward.normalized;
            selectedItem.gameObject.SetActive(true);
            selectedItem.GetComponent<Rigidbody>().AddForce(dropLocation.transform.forward * dropForce, ForceMode.Impulse);
            
            _playerInventory.RemoveItem(selectedItem);
        }
    }
}