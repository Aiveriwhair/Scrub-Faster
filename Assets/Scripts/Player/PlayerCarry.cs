using UnityEngine;

/*
public class PlayerCarry : MonoBehaviour
{
    private PlayerInventory _inventory;
    private LayerMask _maskWhileCarried;
    public Transform handsPosition;
    void Start()
    {
        _inventory = GetComponent<PlayerInventory>();
        _maskWhileCarried = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void Update()
    { 
        UpdateCarriedItem();
    }

    private void UpdateCarriedItem()
    {
        for(int i = 0; i < _inventory.Count(); i++)
        {
            var item = _inventory.GetIndex(i);
            var itemTransform = item.transform;
            itemTransform.position = handsPosition.position;
            itemTransform.rotation = handsPosition.rotation;
            item.gameObject.layer = _maskWhileCarried;
            item.gameObject.SetActive(true);
        }
    }

}
*/