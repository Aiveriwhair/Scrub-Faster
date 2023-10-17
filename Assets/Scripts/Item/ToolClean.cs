using UnityEngine;
public class ToolClean : Tool {
    public override void InteractPrimary() {
        if (!_isPicked) base.InteractPrimary();
        if (!pointer.Raycast(detectionLayer, out var hit))
        {
            return;
        }
        
        var center = hit.point;
        var colliders = Physics.OverlapSphere(center, radius, detectionLayer);
        foreach (var coll in colliders)
        {
            print(coll.gameObject.name);
            var cleanableComponent = coll.gameObject.GetComponent<DirtManager>();
            if (cleanableComponent is not null)
            {
                cleanableComponent.Clean(center, radius);
            }
        }
    }

    public LayerMask detectionLayer;
    public PlayerPointer pointer;
    public float radius = .2f;
}