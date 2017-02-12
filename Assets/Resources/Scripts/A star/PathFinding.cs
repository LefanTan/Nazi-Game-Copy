using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;	

public class PathFinding : MonoBehaviour {

	Grid grid;

	[Inject]
	public void Init(Grid grid){
		this.grid = grid;
	}

	public Vector2[] RequestPath(Vector2 startPos, Vector2 endPos){
		return FindPath (startPos, endPos);
	}

	Vector2[] FindPath(Vector2 startPos, Vector2 targetPos){

		Vector2[] wayPoints = new Vector2[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		//list to be evaluated
		Heap<Node> openList = new Heap<Node> (grid.MaxSize);
		HashSet<Node> closedList = new HashSet<Node> ();
		openList.Add (startNode);

		while (openList.Count > 0) {
			Node currentNode = openList.RemoveFirst ();
//			for (int i = 1; i < openList.Count; i++) {
//				//if fcost is the same, use hcost as tiebreaker, in which the one with lower hcost will be the current node
//				if(openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost ){
//					currentNode = openList [i];
//				}
//			}

			closedList.Add (currentNode);

			if (currentNode == targetNode) {
				pathSuccess = true;
				break;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedList.Contains (neighbour))
					continue;

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openList.Contains (neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance (neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openList.Contains (neighbour))
						openList.Add (neighbour);
					else
						openList.UpdateItem (neighbour);
				}
			}
		}
	
		if (pathSuccess) {
			wayPoints = RetracePath (startNode, targetNode);
		}

		return wayPoints;
	}

	int GetDistance(Node nodeA, Node nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY)
			return 14 * distY + 10 * (distX - distY);
		return 14 * distX + 10 * (distY - distX);

	}

	Vector2[] RetracePath (Node startNode, Node targetNode){
		List<Node> path = new List<Node> ();
		Node currentNode = targetNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		//.path = path;

		Vector2[] waypoints = SimplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;

	}

	Vector2[] SimplifyPath(List<Node> path){
		List<Vector2> wayPoints = new List<Vector2> ();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++) {
			Vector2 directionNew = new Vector2 (path[i].gridX - path[i-1].gridX, path[i].gridY - path[i-1].gridY);
			if (directionOld != directionNew) {
				wayPoints.Add (path [i - 1].worldPos);
			}
			directionOld = directionNew;
		}
		return wayPoints.ToArray ();
	}

	public class Factory : Factory<PathFinding>{}

}
