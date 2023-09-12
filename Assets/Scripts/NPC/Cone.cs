using System.Linq;
using UnityEngine;

[RequireComponent (typeof (MeshFilter), typeof(MeshRenderer))]
public class Cone : MonoBehaviour
{
    public FieldOfView fow;
    public int segments = 20;
    public Material material;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private float _lastRadius;
    private float _lastHeight;

    private void Start()
    {
        _lastRadius = fow.detectionRadius * Mathf.Tan((fow.coneAngle / 2) * Mathf.Deg2Rad);
        _lastHeight = fow.detectionRadius;
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        UpdateFieldOfViewCone();
    }

    private void Update ()
    {
        var newRadius = fow.detectionRadius * Mathf.Tan((fow.coneAngle / 2) * Mathf.Deg2Rad);
        var newHeight = fow.detectionRadius;
        if (_lastHeight != newHeight || _lastRadius != newRadius)
        {
            _lastRadius = newRadius;
            _lastHeight = newHeight;
            UpdateFieldOfViewCone();
        }

        if (material != _meshRenderer.material)
        {
            _meshRenderer.material = material;
        }
        
    }

    private void UpdateFieldOfViewCone()
    {
            transform.localPosition = new Vector3(0,0,fow.detectionRadius / 2);
            var mesh = Create(segments, _lastRadius, _lastHeight);
            _meshFilter.sharedMesh = mesh;
    }
    
    private Mesh Create (int subdivisions, float radius, float height) {
        Mesh mesh = new Mesh();
        Mesh meshIn = new Mesh();

        Vector3[] vertices = new Vector3[subdivisions + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[(subdivisions * 2) * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0f);
        for(int i = 0, n = subdivisions - 1; i < subdivisions; i++) {
            float ratio = (float)i / n;
            float r = ratio * (Mathf.PI * 2f);
            float x = Mathf.Cos(r) * radius;
            float z = Mathf.Sin(r) * radius;
            vertices[i + 1] = new Vector3(x, 0f, z);

            uv[i + 1] = new Vector2(ratio, 0f);
        }
        vertices[subdivisions + 1] = new Vector3(0f, height, 0f);
        uv[subdivisions + 1] = new Vector2(0.5f, 1f);

        // construct bottom

        for(int i = 0, n = subdivisions - 1; i < n; i++) {
            int offset = i * 3;
            triangles[offset] = 0; 
            triangles[offset + 1] = i + 1; 
            triangles[offset + 2] = i + 2;
        }

        // construct sides

        int bottomOffset = subdivisions * 3;
        for(int i = 0, n = subdivisions - 1; i < n; i++) {
            int offset = i * 3 + bottomOffset;
            triangles[offset] = i + 1; 
            triangles[offset + 1] = subdivisions + 1; 
            triangles[offset + 2] = i + 2; 
        }

        mesh.vertices = vertices;
        meshIn.vertices = vertices;
        mesh.uv = uv;
        meshIn.uv = uv;
        mesh.triangles = triangles;
        meshIn.triangles = mesh.triangles.Reverse().ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshIn.RecalculateBounds();
        meshIn.RecalculateNormals();

        return CombineMeshes(mesh, meshIn);
    }
    
    Mesh CombineMeshes(Mesh mesh, Mesh invertedMesh)
    {
        CombineInstance[] combineInstancies = new CombineInstance[2]
        {
            new CombineInstance(){mesh = invertedMesh, transform = Matrix4x4.identity},
            new CombineInstance(){mesh = mesh, transform = Matrix4x4.identity}
        };
        
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstancies);
        return combinedMesh;
    }

}