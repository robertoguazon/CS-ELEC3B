using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : Movement, IPieceMovement {

	public CircleMovement() {
		BoundComputations += ComputeBound;
	}

	public void ComputeBound(Piece piece) {
		GCPlayer player = GameManager.Instance.CurrentPlayer;
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
	}
}
