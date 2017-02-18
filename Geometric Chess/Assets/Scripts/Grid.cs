using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Grid : MonoBehaviour {

	[SerializeField]
	private GameObject tilePrefab;
	[SerializeField]
	private GameObject tilePrefab2;

	[SerializeField]
	private int rows;
	[SerializeField]
	private int cols;
	//[SerializeField]
	//private int height;
	[SerializeField]
	private bool checkDiagonals = true;

	[SerializeField]
	private GameObject[] piecesPrefabs;

	private Node[,] grid;
	private Vector3 tileSize;
	private Vector3 size;

	private Scaler scaler;

	public bool IsReady {
		get{
			for (int row = 0; row < rows; row++) {
				for (int col = 0; col < cols; col++) {
					Node node = grid[row,col];
					if (!node.IsReady) return false;
				}
			}
			return true;
		}
	}

	public int Size {
		get {
			return rows * cols;
		}
	} 

	public Node GetNodeAt(int row, int col) {
		if (row < 0 || row >= rows || col < 0 || col >= cols) return null;
		return grid[row,col];
	}

	public Node GetNodeAt(Vector3 pos) {
		pos -= transform.position;

		float percentRow = (pos.z / size.z) + 0.5f;
		float percentCol = (pos.x / size.x) + 0.5f;
		percentRow = Mathf.Clamp01(percentRow);
		percentCol = Mathf.Clamp01(percentCol);
		int row = Mathf.RoundToInt((rows-1) * percentRow);
		int col = Mathf.RoundToInt((cols-1) * percentCol);

		return grid[row,col];
	}

	void Awake() {
		grid = new Node[rows,cols];
		tileSize =  tilePrefab.GetComponent<Renderer>().bounds.size;
		size = new Vector3(tileSize.x * cols, tileSize.y, tileSize.z * rows);
		scaler = new Scaler();
		CreateGrid();
	}

	void CreateGrid() {
		Vector3 bottomLeft = new Vector3(
				transform.position.x - size.x / 2 + tileSize.x / 2,
				transform.position.y, 
				transform.position.z - size.z / 2 + tileSize.z / 2);
		Vector3 startPosition = bottomLeft;

		GameObject tile = tilePrefab;

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				startPosition.z = bottomLeft.z + tileSize.z * row;
				startPosition.x = bottomLeft.x + tileSize.x * col;
				GameObject go = Instantiate(tile, startPosition, tile.transform.rotation) as GameObject;
				Node dn = go.AddComponent<Node>();
				dn.row = row;
				dn.col = col;
				grid[row,col] = dn;
				go.transform.parent = transform;
				go.transform.localScale = Vector3.zero;

				dn.ScaleIn(Random.Range(0f,1f),Random.Range(1f,2f),1f);
				tile = SwapTilePrefab(tile);
			}
			tile = SwapTilePrefab(tile);
		}

		StartCoroutine(SpawnPieces());
	}

	GameObject SwapTilePrefab(GameObject go) {
		if (tilePrefab == go) return tilePrefab2;
		
		return tilePrefab;
	}

	IEnumerator SpawnPieces() {
		while (!IsReady) {
			yield return null;
		}

		//0 - box
		//1 - triangle
		//2 - circle
		//3 - cross
		//4 - hexagon

		//spawn circles
		for (int i = 1; i <= 6; i++) {
			SpawnPiece(new GridCoords(2,i),piecesPrefabs[2]);
			SpawnPiece(new GridCoords(5,i),piecesPrefabs[2]);
		}

		//spawn boxes
		SpawnPiece(new GridCoords(1,1),piecesPrefabs[0]);
		SpawnPiece(new GridCoords(1,6),piecesPrefabs[0]);
		SpawnPiece(new GridCoords(6,1),piecesPrefabs[0]);
		SpawnPiece(new GridCoords(6,6),piecesPrefabs[0]);

		//spawn triangles
		SpawnPiece(new GridCoords(1,2),piecesPrefabs[1]);
		SpawnPiece(new GridCoords(1,5),piecesPrefabs[1]);
		SpawnPiece(new GridCoords(6,2),piecesPrefabs[1]);
		SpawnPiece(new GridCoords(6,5),piecesPrefabs[1]);

		//spawn crosses
		SpawnPiece(new GridCoords(1,4),piecesPrefabs[3]);
		SpawnPiece(new GridCoords(6,4),piecesPrefabs[3]);

		//spawn hexagons
		SpawnPiece(new GridCoords(1,3),piecesPrefabs[4]);
		SpawnPiece(new GridCoords(6,3),piecesPrefabs[4]);

	}

	public void SpawnPiece(GridCoords coords, GameObject piece) {
		Node pieceNode = GetNodeAt(coords.row, coords.col);
		pieceNode.walkable = false;
		pieceNode.gameObject.layer = LayerMask.NameToLayer("Default");

		GameObject pieceObject = Instantiate(piece, pieceNode.transform.position + Vector3.up * 1.2f, piece.transform.rotation) as GameObject;
		pieceObject.transform.localScale = Vector3.zero;
		ScalableObject so = pieceObject.AddComponent<ScalableObject>();
		
		so.ScaleIn(Random.Range(0f,1f),Random.Range(1f,2f),piece.transform.localScale.x);
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();
		for (int row = -1; row <= 1; row++) {
			for (int col = -1; col <= 1; col++) {

				//skip these ones
				if (row == 0 && col == 0)
					continue;
				if (!checkDiagonals && (row * row) == 1 && (col * col) == 1)
					continue;

				int checkRow = node.row + row;
				int checkCol = node.col + col;

				if (checkRow >= 0 && checkRow < rows && checkCol >= 0 && checkCol < cols) {
					neighbours.Add(grid[checkRow,checkCol]);
				}
			}
		}

		return neighbours;
	}

	public bool IsNearby(Node nodeA, Node nodeB) {
		for (int row = -1; row <= 1; row++) {
			for (int col = -1; col <= 1; col++) {

				//skip these ones
				if (row == 0 && col == 0)
					continue;
				if (!checkDiagonals && (row * row) == 1 && (col * col) == 1)
					continue;

				int checkRow = nodeA.row + row;
				int checkCol = nodeA.col + col;

				if (checkRow >= 0 && checkRow < rows && checkCol >= 0 && checkCol < cols) {
					if (grid[checkRow,checkCol] == nodeB) return true;
				}
			}
		}

		return false;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(2,2,2));
	}
}
