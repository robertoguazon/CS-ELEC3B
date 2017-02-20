using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : Movable, IClickable {

	[SerializeField]
	private Node node;
	[SerializeField]
	private MovementType movementType;
	

	private IPieceMovement pieceMovement;
	private bool dropping;

	protected override void Start() {
		base.Start();
		pieceMovement = Creator.CreatePieceMovement(movementType); //TODO delete
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
		if (node != null) {
			node.Clear();
		}
		node = n;
		n.Piece = this;
	}

	public bool Inform<T>(T arg) {
		//TODO
		return true;
	}

	public void Highlight() {
		SetEmission(GameManager.Instance.PieceHighlightColor);
	}

	public void ZeroGravity() {
		gameObject.GetComponent<Rigidbody>().useGravity = false;
	}

	public void Pickup() {
		Highlight();
		ZeroGravity();
		MoveBy(new Vector3(0,1,0), null);
	}

	public void Drop() {
		dropping = true;
		SetEmissionOriginal();
		StopMoveCoroutine();
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		GCPlayer currPlayer = GameManager.Instance.CurrentPlayer;
		currPlayer.ClearPossibleEats();
		currPlayer.ClearPossibleMoves();
		ready = false;
	}

	//EXPERIMENT
	 void OnCollisionEnter(Collision collision) {
        if (dropping && collision.collider.gameObject) {
			ready = true;
			dropping = false;
		}
    }

}
