using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWith : MonoBehaviour
{
    public Transform followedPosition;
    void Update()
    {
        transform.position = followedPosition.position;
    }
}
