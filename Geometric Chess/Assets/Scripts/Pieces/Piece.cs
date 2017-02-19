using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : Movable, IClickable {

	[SerializeField]
	private Node node;

	private IPieceMovement pieceMovement;

	protected override void Start() {
		base.Start();
		pieceMovement = new CrossMovement(); //TODO delete
	}

	public void Compute() {
		pieceMovement.Compute(this);
	}

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
		print(IsReady);
		if (!IsReady) return false; //EXPERIMENT

		Vector3 prevPos = transform.position;
		ZeroGravity();
		MoveBy(new Vector3(0,1,0));
		SetEmission(GameManager.Instance.PieceHighlightColor);
		return true;
	}

	public void ZeroGravity() {
		gameObject.GetComponent<Rigidbody>().useGravity = false;
	}

	public void Drop() {
		SetEmissionOriginal();
		StopMoveIEnumerator();
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		GCPlayer currPlayer = GameManager.Instance.CurrentPlayer;
		currPlayer.ClearPossibleEats();
		currPlayer.ClearPossibleMoves();
		ready = false;
	}

	//EXPERIMENT
	 void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject) {
			ready = true;
		}
    }

}
