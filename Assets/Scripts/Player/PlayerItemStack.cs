using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemStack : MonoBehaviour
{
    public List<ItemInteractable> stackedItems;
    public Transform stackTransform;

    private void FixedUpdate()
    {
        StackItems();
    }

    private void StackItems()
    {
        var stackPosition = stackTransform.position;
        var stackRotation = stackTransform.rotation;
        foreach (var item in stackedItems)
        {
            var itemSize = item.GetComponent<Renderer>().bounds.size;
            
            item.transform.position = new Vector3(
                stackPosition.x,
                stackPosition.y + itemSize.y / 2,
                stackPosition.z
            );
            item.transform.rotation = stackRotation;
            item.GetComponent<Rigidbody>().useGravity = false;
            stackPosition.y += itemSize.y;
        }
    }
}
