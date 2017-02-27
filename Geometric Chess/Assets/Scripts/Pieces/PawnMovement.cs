using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement, IPieceMovement {

	private bool moved = false;
	private bool didSpecialMove = false;
	private Node[] specialNodes = null;
	private bool turn = true;

	public PawnMovement() {
		BoundComputations += ComputeBound;
		GameManager.SwitchedEvent += OnSwitchEvent;
		specialNodes = new Node[2];
	}

	public void ComputeBound(GCPlayer player, Piece piece) {
		Node currNode = piece.Node;
		int origRow = currNode.row;
		int origCol = currNode.col;
		
		GCPlayer p1 = GameManager.Instance.P1;
		GCPlayer p2 = GameManager.Instance.P2;
		Node frontNode = null;
		Node leftEatNode = null;
		Node rightEatNode = null;
		Grid grid = GameManager.Instance.Grid;

		int toAdd = 0;
		if (p1.Has(piece)) {
			toAdd = 1;
		} else {
			toAdd = -1;
		}

		frontNode = grid.GetNodeAt(origRow + toAdd, origCol);
		leftEatNode = grid.GetNodeAt(origRow + toAdd, origCol - 1);
		rightEatNode = grid.GetNodeAt(origRow + toAdd, origCol + 1);

		ComputeEatPiece(player, piece,leftEatNode);
		ComputeEatPiece(player, piece, rightEatNode);
		ComputeMovePiece(player, piece, frontNode);

		if (!moved && !didSpecialMove) {
			specialNodes[0] = frontNode;
			specialNodes[1] = grid.GetNodeAt(origRow + toAdd * 2, origCol);
			ComputeMovePiece(player,piece, specialNodes[1]);
		}
	}

	public void OnSwitchEvent() {
		turn = !turn;
		if (!turn) return;

		if (moved && didSpecialMove) { // on next move
			Debug.Log("released en passant" );
			if (specialNodes[0] != null) {
				specialNodes[0].Piece = null;
			}
			specialNodes[0] = null;
			specialNodes[1] = null;
		}
	}

	public void Moved(Piece piece) {
		if (specialNodes[0] == null && specialNodes[1] == null) return;

		if (!moved) {
			moved = true;
			if (specialNodes[1] == piece.Node) {
				didSpecialMove = true;
				specialNodes[0].Piece = piece;
			}
		}
	}
}
