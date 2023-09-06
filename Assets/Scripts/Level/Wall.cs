using System;
using UnityEngine;

[Serializable]
public struct Wall
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector2[] openings;
    public FaceType type;
}