using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType {
		P1, P2
}

public class GCPlayer : IClicker, IInputReceiver {

	private PlayerType type;

	private List<Piece> pieces;
	private List<Piece> eatenPieces;

	private Piece piece;
	private Piece checkedBy; //Experimental

	//Experimental
	public bool IsChecked {
		get {return checkedBy != null;}
	}

	//Experimental
	public Piece CheckedBy {
		get {return checkedBy;}
		set {checkedBy = value;}
	}

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
	}

	public void EnableInput() {
		InputManager.InputEvent += OnInputEvent;
	}

	public void DisableInput() {
		InputManager.InputEvent -= OnInputEvent;
	}

	public void OnInputEvent(InputActionType action) {
		switch (action) {
			case InputActionType.GRAB_PIECE:
				Node gNode = Finder.RayHitFromScreen<Node>(Input.mousePosition);
				if (gNode == null) break;
				piece = gNode.Piece;
				if (piece == null) break;
				if (!piece.IsReady) break;
				if (Click(gNode) && piece && Has(piece) && Click(piece)) {
					piece.Pickup();
					piece.Compute();
					piece.HighlightPossibleMoves();
					piece.HighlightPossibleEats();
					GameManager.Instance.GameState.Grab();
				} 

				//check clickable for tile and piece then pass Player
				//check if player has piece - PIECE 
				//check if player has piece if not empty - NODE 
				break;
			case InputActionType.CANCEL_PIECE:
					if (piece != null) {
						//if (!piece.IsReady) break;
						piece.Drop();
						piece = null;
						GameManager.Instance.GameState.Cancel();
					}
				break;
			case InputActionType.PLACE_PIECE:
				Node tNode = Finder.RayHitFromScreen<Node>(Input.mousePosition);
				if (tNode == null) break;
				Piece tPiece = tNode.Piece;
				if (tPiece == null) {
					if (piece.IsPossibleMove(tNode)) {
						Node oldNode = piece.Node;
						piece.UpdateNode(tNode);
						checkedBy = null;
						GameManager.Instance.PlayerOponent.ComputePieces();
						if (IsChecked) {
							piece.UpdateNode(oldNode);
						} else {
							piece.MoveToXZ(tNode, AfterPlacing);
							GameManager.Instance.GameState.Place();
							//piece.Compute(); //Experimental
						}
					}
				} else {
					if (piece.IsPossibleEat(tNode)) {

						Node oldNode = piece.Node;
						Piece ePiece = tNode.Piece;
						piece.UpdateNode(tNode);
						checkedBy = null;
						GameManager.Instance.PlayerOponent.ComputePieces();
						if (IsChecked) {
							piece.UpdateNode(oldNode);
							ePiece.UpdateNode(tNode);
						} else {
							GCPlayer oppPlayer = GameManager.Instance.PlayerOponent;
							oppPlayer.RemovePiece(tPiece);
							AddEatenPieces(tPiece);
							tPiece.ScaleOut(0.2f, 1.5f);
							piece.MoveToXZ(tNode, AfterPlacing);
							GameManager.Instance.GameState.Place();
						}
					}
					//piece.Compute(); //Experimental
				}
				break;
		}
	}

	private void AfterPlacing() {
		piece.Drop();
		GameManager.Instance.GameState.Release();
		piece = null;
	}

	public bool Has(Piece piece) {
		return pieces.Contains(piece);
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

	public bool RemovePiece(Piece piece) {
		return pieces.Remove(piece);
	}

	public void ComputePieces() {
		for (int i = 0; i < pieces.Count; i++) {
			pieces[i].Compute();
		}
	}
}
