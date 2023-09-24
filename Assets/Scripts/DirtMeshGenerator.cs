using UnityEngine;
using System.Collections.Generic;

public class DirtMeshGenerator : MonoBehaviour 
{
	private SquareGrid _squareGrid;
	private List<Vector3> _vertices;
	private List<int> _triangles;

	public Mesh GenerateMesh(int[,] map, float squareSize) {
		_squareGrid = new SquareGrid(map, squareSize);

		_vertices = new List<Vector3>();
		_triangles = new List<int>();

		for (int x = 0; x < _squareGrid.squares.GetLength(0); x ++) {
			for (int y = 0; y < _squareGrid.squares.GetLength(1); y ++) {
				TriangulateSquare(_squareGrid.squares[x,y]);
			}
		}

		var mesh = new Mesh
		{
			vertices = _vertices.ToArray(),
			triangles = _triangles.ToArray()
		};
		mesh.RecalculateNormals();
		return mesh;
	}

	private void TriangulateSquare(Square square) {
		switch (square.Configuration) {
		case 0:
			break;

		// 1 points:
		case 1:
			MeshFromPoints(square.Bottom, square.BottomLeft, square.Left);
			break;
		case 2:
			MeshFromPoints(square.Right, square.BottomRight, square.Bottom);
			break;
		case 4:
			MeshFromPoints(square.Top, square.TopRight, square.Right);
			break;
		case 8:
			MeshFromPoints(square.TopLeft, square.Top, square.Left);
			break;

		// 2 points:
		case 3:
			MeshFromPoints(square.Right, square.BottomRight, square.BottomLeft, square.Left);
			break;
		case 6:
			MeshFromPoints(square.Top, square.TopRight, square.BottomRight, square.Bottom);
			break;
		case 9:
			MeshFromPoints(square.TopLeft, square.Top, square.Bottom, square.BottomLeft);
			break;
		case 12:
			MeshFromPoints(square.TopLeft, square.TopRight, square.Right, square.Left);
			break;
		case 5:
			MeshFromPoints(square.Top, square.TopRight, square.Right, square.Bottom, square.BottomLeft, square.Left);
			break;
		case 10:
			MeshFromPoints(square.TopLeft, square.Top, square.Right, square.BottomRight, square.Bottom, square.Left);
			break;

		// 3 point:
		case 7:
			MeshFromPoints(square.Top, square.TopRight, square.BottomRight, square.BottomLeft, square.Left);
			break;
		case 11:
			MeshFromPoints(square.TopLeft, square.Top, square.Right, square.BottomRight, square.BottomLeft);
			break;
		case 13:
			MeshFromPoints(square.TopLeft, square.TopRight, square.Right, square.Bottom, square.BottomLeft);
			break;
		case 14:
			MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.Bottom, square.Left);
			break;

		// 4 point:
		case 15:
			MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
			break;
		}
	}

	private void MeshFromPoints(params Node[] points) {
		AssignVertices(points);

		if (points.Length >= 3)
			CreateTriangle(points[0], points[1], points[2]);
		if (points.Length >= 4)
			CreateTriangle(points[0], points[2], points[3]);
		if (points.Length >= 5) 
			CreateTriangle(points[0], points[3], points[4]);
		if (points.Length >= 6)
			CreateTriangle(points[0], points[4], points[5]);

	}

	private void AssignVertices(Node[] points) {
		for (int i = 0; i < points.Length; i ++) {
			if (points[i].VertexIndex == -1) {
				points[i].VertexIndex = _vertices.Count;
				_vertices.Add(points[i].Position);
			}
		}
	}

	private void CreateTriangle(Node a, Node b, Node c) {
		_triangles.Add(a.VertexIndex);
		_triangles.Add(b.VertexIndex);
		_triangles.Add(c.VertexIndex);
	}
	
	public class SquareGrid {
		public Square[,] squares;

		public SquareGrid(int[,] map, float squareSize) {
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);
			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for (int x = 0; x < nodeCountX; x ++) {
				for (int y = 0; y < nodeCountY; y ++) {
					Vector3 pos = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
					controlNodes[x,y] = new ControlNode(pos,map[x,y] == 1, squareSize);
				}
			}

			squares = new Square[nodeCountX -1,nodeCountY -1];
			for (int x = 0; x < nodeCountX-1; x ++) {
				for (int y = 0; y < nodeCountY-1; y ++) {
					squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
				}
			}

		}
	}
	
	public class Square {

		public ControlNode TopLeft, TopRight, BottomRight, BottomLeft;
		public Node Top, Right, Bottom, Left;
		public int Configuration;

		public Square (ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft) {
			TopLeft = topLeft;
			TopRight = topRight;
			BottomRight = bottomRight;
			BottomLeft = bottomLeft;

			Top = topLeft.Right;
			Right = bottomRight.Above;
			Bottom = bottomLeft.Right;
			Left = bottomLeft.Above;

			if (TopLeft.Active)
				Configuration += 8;
			if (TopRight.Active)
				Configuration += 4;
			if (BottomRight.Active)
				Configuration += 2;
			if (BottomLeft.Active)
				Configuration += 1;
		}
	}

	public class Node {
		public Vector3 Position;
		public int VertexIndex = -1;

		public Node(Vector3 pos) {
			Position = pos;
		}
	}

	public class ControlNode : Node {

		public bool Active;
		public Node Above, Right;

		public ControlNode(Vector3 pos, bool active, float squareSize) : base(pos) {
			Active = active;
			Above = new Node(Position + Vector3.forward * squareSize/2f);
			Right = new Node(Position + Vector3.right * squareSize/2f);
		}

	}
	
	/*
	void OnDrawGizmos() {
		
		if (_squareGrid != null) {
			for (int x = 0; x < _squareGrid.squares.GetLength(0); x ++) {
				for (int y = 0; y < _squareGrid.squares.GetLength(1); y ++) {

					Gizmos.color = (_squareGrid.squares[x,y].topLeft.active)?Color.black:Color.white;
					Gizmos.DrawCube(_squareGrid.squares[x,y].topLeft.position, Vector3.one * .4f);

					Gizmos.color = (_squareGrid.squares[x,y].topRight.active)?Color.black:Color.white;
					Gizmos.DrawCube(_squareGrid.squares[x,y].topRight.position, Vector3.one * .4f);

					Gizmos.color = (_squareGrid.squares[x,y].bottomRight.active)?Color.black:Color.white;
					Gizmos.DrawCube(_squareGrid.squares[x,y].bottomRight.position, Vector3.one * .4f);

					Gizmos.color = (_squareGrid.squares[x,y].bottomLeft.active)?Color.black:Color.white;
					Gizmos.DrawCube(_squareGrid.squares[x,y].bottomLeft.position, Vector3.one * .4f);


					Gizmos.color = Color.grey;
					Gizmos.DrawCube(_squareGrid.squares[x,y].centreTop.position, Vector3.one * .15f);
					Gizmos.DrawCube(_squareGrid.squares[x,y].centreRight.position, Vector3.one * .15f);
					Gizmos.DrawCube(_squareGrid.squares[x,y].centreBottom.position, Vector3.one * .15f);
					Gizmos.DrawCube(_squareGrid.squares[x,y].centreLeft.position, Vector3.one * .15f);

				}
			}
		}
	}
	*/
}