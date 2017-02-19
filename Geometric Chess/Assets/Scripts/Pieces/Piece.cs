using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : ScalableObject, IClickable {

	[SerializeField]
	private Node node;

	public string ChessCoords {
		get {
			if (node == null) return null;

			return node.ChessCoords;
		}
	}

	public Node Node {
		get {return node;}
	}

	public void UpdateNode(Node n) {
		node = n;
		n.Piece = this;
	}

	public bool Inform<T>(T arg) {
		Vector3 prevPos = transform.position;
		transform.position = new Vector3(prevPos.x,prevPos.y + 1,prevPos.z);
		return true;
	}

}
