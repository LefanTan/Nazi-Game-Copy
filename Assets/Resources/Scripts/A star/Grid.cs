using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Grid : MonoBehaviour {

	public bool onDisplayPathGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public List<Node> path;

	Node[,] grid;
	int gridAmountX, gridAmountY;
	float nodeDiameter;

	void Awake(){
		nodeDiameter = nodeRadius * 2;
		gridAmountX = Mathf.RoundToInt (gridWorldSize.x/nodeDiameter);
		gridAmountY = Mathf.RoundToInt (gridWorldSize.y/nodeDiameter);

		CreateGrid ();
	}

	private void CreateGrid(){
		grid = new Node[gridAmountX, gridAmountY];
		Vector2 worldBottomLeft = (Vector2)transform.position - (Vector2.right * (gridWorldSize.x / 2)) - (Vector2.up * (gridWorldSize.y / 2));

		for (int x = 0; x < gridAmountX; x++) {
			for (int y = 0; y < gridAmountY; y++) {
				Vector2 worldPos = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(worldPos, nodeRadius, unwalkableMask));

				grid [x, y] = new Node (worldPos, walkable, x, y);
			}
		}


	}

	public int MaxSize{
		get{ 
			return gridAmountX * gridAmountY;
		}
	}

	public Node NodeFromWorldPoint(Vector2 pos){
		float percentX = (pos.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (pos.y + gridWorldSize.y / 2) / gridWorldSize.y;

		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt ((gridAmountX - 1) * percentX);
		int y = Mathf.RoundToInt ((gridAmountY - 1) * percentY);

		return grid [x, y];
	}

	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridAmountX && checkY >= 0 && checkY < gridAmountY) {
					neighbours.Add (grid [checkX, checkY]);
				}
			}
		}
		return neighbours;
	}


	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector2 (gridWorldSize.x, gridWorldSize.y));

		if (path != null) {
			if (onDisplayPathGizmos) {
				foreach (Node n in grid) {
					if (path.Contains (n)) {
						Gizmos.color = Color.black;
						Gizmos.DrawCube (n.worldPos, Vector2.one * (nodeDiameter - .05f));
					}
				}
			} else {
				foreach (Node n in grid) {
					Gizmos.color = (n.walkable) ? Color.white : Color.red;
					if (path.Contains (n)) {
						Gizmos.color = Color.black;
					}
					Gizmos.DrawCube (n.worldPos, Vector2.one * (nodeDiameter - .05f));
				}
			}
		}
	}

}
