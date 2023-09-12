using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI() {
        FieldOfView fow = (FieldOfView)target;
        var fowPosition = fow.transform.position;
        
        Handles.color = Color.white;
        if (fow.visibleTargets.Count > 0)
        {
            Handles.color = Color.green;
        }
        Handles.DrawWireArc(fowPosition, Vector3.up, Vector3.forward, 360, fow.detectionRadius, 2);
        Handles.DrawWireArc(fowPosition,  Vector3.left, Vector3.forward, 360, fow.detectionRadius, 2);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.coneAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.coneAngle / 2, false);
        Handles.DrawLine (fowPosition, fowPosition + viewAngleA * fow.detectionRadius, 2);
        Handles.DrawLine (fowPosition, fowPosition + viewAngleB * fow.detectionRadius, 2);
        
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets) {
            Handles.DrawLine (fow.transform.position, visibleTarget.position);
        }
    }

}