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

	protected void OnInputEvent(InputActionType action) {
		switch (action) {
			case InputActionType.GRAB_PIECE:
				Piece piece = Finder.RayHitFromScreen<Piece>(Input.mousePosition);
				if (piece) {
					if (Click(piece)) {
						Debug.Log(piece.ChessCoords);
					}
				}
				break;
		}
	}

	public bool Click(IClickable clickable) {
		if (clickable == null) return false;
		clickable.Inform<GCPlayer>(this); 
		return true;
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
}
