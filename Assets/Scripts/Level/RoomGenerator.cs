using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public float roomWidth;
    public float roomHeight;
    public float roomLength;

    public Wall[] walls;
    
    private MeshFilter _meshFilter;
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        GenerateRoom();
    }

    private void Update()
    {
        StartCoroutine(visualUpdate());
    }

    IEnumerator visualUpdate()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        GenerateRoom();
        yield return new WaitForSeconds(.5f);
    }
    
    void GenerateRoom()
    {
        //var bottomOpenings = new Vector2[] { };
        //var topOpenings = new Vector2[] { };
        //var leftOpenings = new Vector2[] { };
        //var rightOpenings = new Vector2[] { };
        //var frontOpenings = new Vector2[] { };
        //var backOpenings = new Vector2[] { };
        
        var parentTransform = transform;
        foreach (var wall in walls)
        {
            CreateWall(parentTransform, FaceType2Position(wall.type), wall.type, FaceType2Rotation(wall.type), wall.openings);
        }
    }
    void CreateWall(Transform parent, Vector3 position, FaceType type, Vector3 rotation, Vector2[] openings)
    {
        var wallObject = new GameObject("Wall")
        {
            transform =
            {
                parent = parent,
                localPosition = position,
                localRotation = Quaternion.Euler(rotation)
            }
        };
        

        Mesh wallMesh = GeneratePlaneMesh(Vector2.zero, FaceType2WidthHeight(type), new Vector2(1f, 1f), openings, Vector3.zero);

        MeshFilter meshFilter = wallObject.AddComponent<MeshFilter>();
        meshFilter.mesh = wallMesh;

        wallObject.AddComponent<MeshRenderer>();
    }
    
    Mesh GeneratePlaneMesh(Vector2 position, Vector2 dimensions, Vector2 tilesDimensions, Vector2[] openings, Vector3 rotations)
    {
        Mesh planeMesh = new Mesh();

        // Calculez le nombre de tuiles en largeur et en longueur
        int numTilesX = Mathf.FloorToInt(dimensions.x / tilesDimensions.x);
        int numTilesY = Mathf.FloorToInt(dimensions.y / tilesDimensions.y);

        // Calculez le nombre total de tuiles
        int totalTiles = numTilesX * numTilesY;

        // Calculez le nombre de vertices et de triangles nécessaires
        int numVertices = totalTiles * 4; // 4 vertices par tuile (coin en bas à gauche, coin en bas à droite, coin en haut à gauche, coin en haut à droite)
        int numTriangles = totalTiles * 2; // 2 triangles par tuile

        // Initialisez les tableaux de vertices, triangles et UVs
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles * 3]; // Chaque triangle a 3 indices
        Vector2[] uvs = new Vector2[numVertices];

        // Calcul de l'index actuel dans les tableaux
        int vertexIndex = 0;
        int triangleIndex = 0;

        // Calculez la position de départ
        Vector2 currentTilePosition = position;

        // Parcourez toutes les tuiles
        for (int y = 0; y < numTilesY; y++)
        {
            for (int x = 0; x < numTilesX; x++)
            {
                // Vérifiez si cette tuile est une ouverture
                bool isOpening = false;
                foreach (Vector2 opening in openings)
                {
                    if (opening.x == x && opening.y == y)
                    {
                        isOpening = true;
                        break;
                    }
                }

                // Calculez les coins de la tuile
                Vector3 bottomLeft = new Vector3(currentTilePosition.x, 0, currentTilePosition.y);
                Vector3 bottomRight = new Vector3(currentTilePosition.x + tilesDimensions.x, 0, currentTilePosition.y);
                Vector3 topLeft = new Vector3(currentTilePosition.x, 0, currentTilePosition.y + tilesDimensions.y);
                Vector3 topRight = new Vector3(currentTilePosition.x + tilesDimensions.x, 0, currentTilePosition.y + tilesDimensions.y);

                // Ajoutez les vertices pour cette tuile
                vertices[vertexIndex++] = bottomLeft;
                vertices[vertexIndex++] = bottomRight;
                vertices[vertexIndex++] = topLeft;
                vertices[vertexIndex++] = topRight;

                // Ajoutez les triangles pour cette tuile
                if (!isOpening)
                {
                    triangles[triangleIndex++] = vertexIndex - 4;
                    triangles[triangleIndex++] = vertexIndex - 3;
                    triangles[triangleIndex++] = vertexIndex - 2;
                    triangles[triangleIndex++] = vertexIndex - 2;
                    triangles[triangleIndex++] = vertexIndex - 3;
                    triangles[triangleIndex++] = vertexIndex - 1;
                }

                // Calculez les coordonnées UV (peut-être 0,0 pour chaque coin)
                uvs[vertexIndex - 4] = new Vector2(0, 0);
                uvs[vertexIndex - 3] = new Vector2(1, 0);
                uvs[vertexIndex - 2] = new Vector2(0, 1);
                uvs[vertexIndex - 1] = new Vector2(1, 1);

                // Mettez à jour la position actuelle pour la prochaine tuile
                currentTilePosition.x += tilesDimensions.x;
            }

            // Réinitialisez la position en X pour la prochaine ligne de tuiles
            currentTilePosition.x = position.x;

            // Déplacez la position actuelle vers le haut pour la prochaine ligne de tuiles
            currentTilePosition.y += tilesDimensions.y;
        }

        // Appliquez les rotations au tableau de vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Quaternion.Euler(rotations) * vertices[i];
        }

        // Attribuez les vertices, triangles et UVs au maillage de la pièce
        planeMesh.vertices = vertices;
        planeMesh.triangles = triangles;
        planeMesh.uv = uvs;

        // Recalculez les normales pour l'éclairage
        planeMesh.RecalculateNormals();

        return planeMesh;
    }


    public Vector3 FaceType2Position(FaceType type)
    {
        switch (type)
        {
            case FaceType.Back:
                return new Vector3(roomLength, roomHeight, 0);
            case FaceType.Front:
                return new Vector3(0, roomHeight, roomWidth);
            case FaceType.Left:
                return Vector3.zero;
            case FaceType.Right:
                return new Vector3(0, roomHeight, roomWidth);
            case FaceType.Top:
                return new Vector3(0, roomHeight, 0);
            case FaceType.Bottom:
                return new Vector3(roomLength, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    public Vector3 FaceType2Rotation(FaceType type)
    {
        switch (type)
        {
            case FaceType.Back:
                return new Vector3(180, 180, 90);
            case FaceType.Front:
                return new Vector3(180, 0, 90);
            case FaceType.Left:
                return new Vector3(270, 0, 0);
            case FaceType.Right:
                return new Vector3(90, 0, 0);
            case FaceType.Top:
                return Vector3.zero;
            case FaceType.Bottom:
                return new Vector3(0, 0, 180);
            default:
                return Vector3.zero;
        }
    }
    
    public Vector2 FaceType2WidthHeight(FaceType type)
    {
        switch (type)
        {
            case FaceType.Back:
            case FaceType.Front:
                return new Vector2(roomHeight, roomWidth);
            case FaceType.Left:
            case FaceType.Right:
                return new Vector2(roomLength, roomHeight);
            case FaceType.Top:
            case FaceType.Bottom:
                return new Vector2(roomLength, roomWidth);
            default:
                return Vector2.zero;
        }
    }

}
