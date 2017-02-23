using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KingMovement : Movement, IPieceMovement {

	public KingMovement() {
		BoundComputations += ComputeBound;
	}

	public void ComputeBound(Piece piece) {
		GCPlayer player = GameManager.Instance.CurrentPlayer;
		Node currNode = piece.Node;
		int origRow = currNode.row;
		int origCol = currNode.col;
		for (int row = -1; row <= 1; row++) {
			for (int col = -1; col <= 1; col++) {
				if (Mathf.Abs(row) == Mathf.Abs(col)) continue;
				if (row == 0 && col == 0) continue;

				int newRow = origRow + row;
				int newCol = origCol + col;
				ComputeMoveOrEatPiece(player, piece, GameManager.Instance.Grid.GetNodeAt(newRow, newCol));
			}
		}
	}


}
