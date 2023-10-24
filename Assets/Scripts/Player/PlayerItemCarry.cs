using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemCarry : MonoBehaviour
{
    public Transform handPosition;
    
    private PlayerInventory _inventory;
    private ItemPickable _currentItem;

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
        if (_inventory == null) throw new MissingComponentException("GameObject must have a PlayerInventory component");
    }

    public void Update()
    {
        if (_currentItem != null)
        {
            var currentItemTransform = _currentItem.transform;
            currentItemTransform.localPosition = _currentItem.holdingPositionOffset;
            currentItemTransform.localRotation = Quaternion.Euler(_currentItem.holdingRotationEulerAngles);
        }
    }

    public void ManualUpdate()
    {
        ItemPickable selectedItem = _inventory.GetSelectedItem();
        if (selectedItem != _currentItem)
        {
            if (_currentItem != null)
            {
                _currentItem.transform.SetParent(null);
                _currentItem.gameObject.SetActive(false);
            }

            _currentItem = selectedItem;

            if (_currentItem != null)
            {
                _currentItem.gameObject.SetActive(true);
                var currentItemTransform = _currentItem.transform;
                currentItemTransform.SetParent(handPosition);
                currentItemTransform.localPosition = _currentItem.holdingPositionOffset;
                currentItemTransform.localRotation = Quaternion.Euler(_currentItem.holdingRotationEulerAngles);
            }
        }
    }

}