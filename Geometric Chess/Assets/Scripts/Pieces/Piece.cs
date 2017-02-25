﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType {
	NONE,
	CIRCLE,
	TRIANGLE,
	SQUARE,
	HEXAGON,
	CROSS,
	RECTANGLE, //Sample
}

public class Piece : Movable, IClickable {

	[SerializeField]
	private PieceType pieceType;
	[SerializeField]
	private Node node;
	[SerializeField]
	private MovementType movementType;
	

	private IPieceMovement pieceMovement;
	private bool dropping;

	private List<Node> possibleMoves;
	private List<Node> possibleEats;
	private Piece check;

	public bool Checking {
		get {return check != null;}
	}

	public Piece Check {
		get {return check;}
		set {check = value;}
	}

	public List<Node> PossibleMoves {
		get { return possibleMoves;}
	}

	public List<Node> PossibleEats {
		get {return possibleEats;}
	}

	public PieceType PieceType {
		get {return pieceType;}
	}

	public MovementType MovementType {
		get {return movementType;}
	}

	void Awake() {
		possibleEats = new List<Node>();
		possibleMoves = new List<Node>();
	}


	protected override void Start() {
		base.Start();
		pieceMovement = Creator.CreatePieceMovement(movementType); //TODO delete
	}

	public void HighlightPossibleMoves() {
		for (int i = 0; i < possibleMoves.Count; i++) {
			possibleMoves[i].HighlightMove();
		}
	}

	public void UnHighlightPossibleMoves() {
		for (int i = 0; i < possibleMoves.Count; i++) {
			possibleMoves[i].UnhighlightMove();
		}
	}

	public void HighlightPossibleEats() {
		for (int i = 0; i < possibleEats.Count; i++) {
			possibleEats[i].HighlightEat();
		}
	}

	public void UnHighlightPossibleEats() {
		for (int i = 0; i < possibleEats.Count; i++) {
			possibleEats[i].UnhighlightEat();
		}
	}

	public bool IsPossibleMove(Node node) {
		return this.possibleMoves.Contains(node);
	}

	public bool IsPossibleEat(Node node) {
		return this.possibleEats.Contains(node);
	}

	public void AddPossibleMoves(params Node[] nodes) {
		for (int i = 0; i < nodes.Length; i++) {
			this.possibleMoves.Add(nodes[i]);
		}
	}

	public void AddPossibleEats(params Node[] nodes) {
		for (int i = 0; i < nodes.Length; i++) {
			this.possibleEats.Add(nodes[i]);
		}
	}

	public void ClearPossibleMoves() {
		while (possibleMoves.Count > 0) {
			Node node = possibleMoves[0];
			node.UnhighlightMove();
			possibleMoves.Remove(node);
		}
	}

	public void ClearPossibleEats() {
		while (possibleEats.Count > 0) {
			Node node = possibleEats[0];
			node.UnhighlightEat();
			possibleEats.Remove(node);
		}
	}

	public void ClearCheck() {
		if (check != null) {
			node.UnhighlightCheck();
			check.Node.UnhighlightCheck();
			check = null;
		}
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
		//GCPlayer currPlayer = GameManager.Instance.CurrentPlayer;
		UnHighlightPossibleEats();
		UnHighlightPossibleMoves();
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
