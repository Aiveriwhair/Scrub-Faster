using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI() {
        FieldOfView fow = (FieldOfView)target;
        var fowPosition = fow.transform.position;
        Handles.color = Color.white;
        Handles.DrawWireArc (fowPosition, Vector3.up, Vector3.forward, 360, fow.coneRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.coneAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.coneAngle / 2, false);

        
        Handles.DrawLine (fowPosition, fowPosition + viewAngleA * fow.coneRadius);
        Handles.DrawLine (fowPosition, fowPosition + viewAngleB * fow.coneRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets) {
            Handles.DrawLine (fow.transform.position, visibleTarget.position);
        }
    }

}