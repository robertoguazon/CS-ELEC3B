using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType {
		P1, P2
}

public class GCPlayer : IClicker {

	private PlayerType type;

	private List<Piece> pieces;
	private List<Piece> eatenPieces;
	private List<Node> possibleMoves;
	private List<Node> possibleEats;

	private Piece piece;

	public Piece HoldingPiece {
		get {return piece;}
	}

	public bool IsReady {
		get {
			for (int i = 0; i < pieces.Count; i++) {
				if (!pieces[i].IsReady) return false;
			}

			return true;
		}
	}

	public PlayerType Type {
		get {return type;}
	}

	public GCPlayer(PlayerType type) {
		this.type = type;
		pieces = new List<Piece>();
		eatenPieces = new List<Piece>();
		possibleEats = new List<Node>();
		possibleMoves = new List<Node>();
	}

	public void EnableInput() {
		InputManager.InputEvent += OnInputEvent;
	}

	public void DisableInput() {
		InputManager.InputEvent -= OnInputEvent;
	}

	protected void OnInputEvent(InputActionType action) {
		switch (action) {
			case InputActionType.GRAB_PIECE:
				Piece piece = Finder.RayHitFromScreen<Piece>(Input.mousePosition);
				if (piece && Has(piece) && Click(piece)) {
					this.piece = piece;
					Debug.Log(piece.ChessCoords);
					GameManager.Instance.GameState.Grabbed();
					piece.Compute();
				}
				break;
			case InputActionType.CANCEL_PIECE:
					this.piece.Drop();
					this.piece = null;
					GameManager.Instance.GameState.Released();
				break;
		}
	}

	public bool Has(Piece piece) {
		return pieces.Contains(piece);
	}

	public bool IsPossibleMove(Node node) {
		return this.possibleMoves.Contains(node);
	}

	public bool IsPossibleEat(Node node) {
		return this.possibleEats.Contains(node);
	}

	public bool Click(IClickable clickable) {
		if (clickable == null) return false;
		return clickable.Inform<GCPlayer>(this); 
	}

	public void AddPieces(params Piece[] pieces) {
		for (int i = 0; i < pieces.Length; i++) {
			this.pieces.Add(pieces[i]);
		}
	}

	public void AddEatenPieces(params Piece[] pieces) {
		for (int i = 0; i < pieces.Length; i++) {
			this.eatenPieces.Add(pieces[i]);
		}
	}

	public bool EatPiece(Piece piece) {
		return pieces.Remove(piece);
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
			node.SetMaterialOriginal();
			possibleMoves.Remove(node);
		}
	}

	public void ClearPossibleEats() {
		while (possibleEats.Count > 0) {
			Node node = possibleEats[0];
			node.SetMaterialOriginal();
			possibleEats.Remove(node);
		}
	}
}
