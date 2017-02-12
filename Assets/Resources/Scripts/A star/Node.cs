using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{

	public Node parent;
	public Vector2 worldPos;
	public bool walkable;
	public int gCost;
	public int hCost;

	//it's array pos in the grid[,]
	public int gridX;
	public int gridY;

	private int heapIndex;

	public Node(Vector2 worldPos, bool walkable, int gridX, int gridY){
		this.worldPos = worldPos;
		this.walkable = walkable;
		this.gridX = gridX;
		this.gridY = gridY;
	}

	public int FCost{
		get{ return hCost + gCost; }
	}

	public int HeapIndex{
		get{ 
			return heapIndex;
		}
		set{ 
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare){
		int compare = FCost.CompareTo (nodeToCompare.FCost);
		if (compare == 0) {
			compare = hCost.CompareTo (nodeToCompare.hCost);
		}

		return -compare;
	}

}
