using System.Collections.Generic;
using UnityEngine;




public class DirtManager : MonoBehaviour
{
    
    private Mesh _dirtMesh;
    private MeshCollider _dirtCollider;
    private bool _isClean;
    private ParticleSystem _particleSystem;


    [Header("Cellular automata")]
    [Range(0, 100)]
    public int randomFillPercent;
    public int numIterations;


    [Header("Mesh Generation")]
    public float squareSize;
    
    [Header("Dirt settings")]
    public int gridSizeX = 20;
    public int gridSizeY = 20;
    
    

    private void Awake()
    {
        _isClean = false;
        _dirtCollider = gameObject.AddComponent<MeshCollider>();
        if(_dirtCollider == null)  throw new MissingComponentException("MeshCollider needed");
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        if (_particleSystem == null) throw new MissingComponentException("A Particle System component is required");
        _particleSystem.Stop();
    }

    void Start()
    {
        GenerateDirtMesh();
        GetComponent<MeshFilter>().mesh = _dirtMesh;
        _dirtCollider.sharedMesh = _dirtMesh;
    }

    private void GenerateDirtMesh()
    {
        var grid = GetComponent<CellularAutomaton>().GenerateMap(gridSizeX, gridSizeY, randomFillPercent, numIterations);
        _dirtMesh = (GetComponent<DirtMeshGenerator>().GenerateMesh(grid, squareSize));
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
            _particleSystem.Play();
        }

        _dirtCollider.sharedMesh = _dirtMesh;
        GetComponent<MeshFilter>().mesh = _dirtMesh;
    }
}
