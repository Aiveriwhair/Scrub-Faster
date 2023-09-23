using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DirtManager : MonoBehaviour
{
    
    private Mesh _dirtMesh;
    private MeshCollider _dirtCollider;
    private bool _isClean;


    [Header("Dirt settings")]
    public int gridSizeX = 20;
    public int gridSizeY = 20;
    public float cellSize = .05f;
    public float scale = .08f;
    public int seed;
    public float dirtHeight = .01f;

    private void Awake()
    {
        _isClean = false;
        seed = 0;
        _dirtCollider = gameObject.AddComponent<MeshCollider>();
    }

    void Start()
    {
        GenerateDirtMesh();
        GetComponent<MeshFilter>().mesh = _dirtMesh;
        _dirtCollider.sharedMesh = _dirtMesh;
    }
    public void GenerateDirtMesh()
    {
        _dirtMesh = new Mesh();

        Random.InitState(seed);

        Vector3[] vertices = new Vector3[(gridSizeX + 1) * (gridSizeY + 1)];
        int[] triangles = new int[gridSizeX * gridSizeY * 6];

        for (int y = 0; y <= gridSizeY; y++)
        {
            for (int x = 0; x <= gridSizeX; x++)
            {
                float xOffset = Random.Range(-cellSize * 0.2f, cellSize * 0.2f);
                float yOffset = Random.Range(-cellSize * 0.2f, cellSize * 0.2f);
                float height = Mathf.PerlinNoise((x + xOffset) * scale, (y + yOffset) * scale);

                height = Mathf.Round(height) * dirtHeight;

                vertices[y * (gridSizeX + 1) + x] = new Vector3(x * cellSize, height, y * cellSize);
            }
        }

        int triangleIndex = 0;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                int vertexIndex = y * (gridSizeX + 1) + x;

                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + gridSizeX + 1;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + gridSizeX + 1;
                triangles[triangleIndex + 5] = vertexIndex + gridSizeX + 2;

                triangleIndex += 6;
            }
        }

        List<int> validTriangles = new();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            var vertexIndex1 = triangles[i];
            var vertexIndex2 = triangles[i + 1];
            var vertexIndex3 = triangles[i + 2];

            var height1 = vertices[vertexIndex1].y;
            var height2 = vertices[vertexIndex2].y;
            var height3 = vertices[vertexIndex3].y;

            if (Mathf.Approximately(height1, dirtHeight) && Mathf.Approximately(height2, dirtHeight) && Mathf.Approximately(height3, dirtHeight))
            {
                continue;
            }

            validTriangles.Add(vertexIndex1);
            validTriangles.Add(vertexIndex2);
            validTriangles.Add(vertexIndex3);
        }

        _dirtMesh.vertices = vertices;
        _dirtMesh.triangles = validTriangles.ToArray();
        _dirtMesh.RecalculateNormals();
    }

    public bool IsClean()
    {
        return _isClean;
    }

    public void Clean(Vector3 hitPoint, float radius)
    {
        if (IsClean()) return;
        
        var triangles = _dirtMesh.triangles;
        var vertices = _dirtMesh.vertices;

        var newTriangles = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            var triangleIndex1 = triangles[i];
            var triangleIndex2 = triangles[i + 1];
            var triangleIndex3 = triangles[i + 2];

            var vertex1 = transform.TransformPoint(vertices[triangleIndex1]);
            var vertex2 = transform.TransformPoint(vertices[triangleIndex2]);
            var vertex3 = transform.TransformPoint(vertices[triangleIndex3]);
            
            if (Vector3.Distance(vertex1, hitPoint) > radius &&
                Vector3.Distance(vertex2, hitPoint) > radius &&
                Vector3.Distance(vertex3, hitPoint) > radius)
            {
                newTriangles.Add(triangleIndex1);
                newTriangles.Add(triangleIndex2);
                newTriangles.Add(triangleIndex3);
            }
        }

        _dirtMesh.triangles = newTriangles.ToArray();
        if (newTriangles.Count <= 0)
        {
            _isClean = true;
            Debug.Log("Saleté cleaned");
        }
        

        _dirtCollider.sharedMesh = _dirtMesh;
        GetComponent<MeshFilter>().mesh = _dirtMesh;
    }
}
