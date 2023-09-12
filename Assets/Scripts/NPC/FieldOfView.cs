using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class FieldOfView : MonoBehaviour
{
    public float detectionRadius = 10f;
    [Range(0, 360)]
    public float coneAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public void Start()
    {
        StartCoroutine(FindTargetsWithDelay(.2f));
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            DrawLineToTargets();
        }
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        var targetsInViewRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < coneAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private void DrawLineToTargets()
    {
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            Color color = new Color(255, 0, 0);
            Debug.DrawLine(this.transform.position, visibleTargets[i].position, color);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}