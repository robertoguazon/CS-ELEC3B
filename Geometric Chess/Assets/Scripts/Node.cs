using UnityEngine;
using System.Collections;

public class Node : ScalableObject, IHeapItem<Node>, IClickable {
	public int row;
	public int col;

	public bool walkable = true;
	
	public int gCost;
	public int hCost;
	
	public Node parent;
	private int heapIndex;

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		} 
		set {
			heapIndex = value;
		}
	}

	public bool Inform(object arg) {
		//TODO
		return true;
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}

		return compare;
	}

	public override string ToString() {
		return "" + row + "x" + col;
	}
}
