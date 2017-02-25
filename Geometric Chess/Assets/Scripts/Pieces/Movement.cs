using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
	NONE,
	KING,
	PAWN,
	ROOK,
	BISHOP,
	QUEEN,
	KNIGHT,
}

public abstract class Movement : ScriptableObject {

	
	public event ComputeBound BoundComputations;

	public Movement() {
		BoundComputations += ClearPossibles;
	}

	public void ClearPossibles(Piece piece) {
		piece.ClearPossibleEats();
		piece.ClearPossibleMoves();
	}

	public bool ComputeMovePiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (toCheckNode.EmptySpace) {
			playerPiece.AddPossibleMoves(toCheckNode);
			return true;
		}

		return false;
	}

	public bool ComputeEatPiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (!toCheckNode.EmptySpace && Rules.IsEnemy(playerPiece, toCheckNode.Piece)) {
			AddToCheckOrEat(playerPiece, toCheckNode.Piece);
			return true;
		}

		return false;
	}

	public bool ComputeMoveOrEatPiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (toCheckNode.EmptySpace) {
			playerPiece.AddPossibleMoves(toCheckNode);
			return true;
		} else if (Rules.IsEnemy(playerPiece, toCheckNode.Piece)) {
			AddToCheckOrEat(playerPiece, toCheckNode.Piece);
			return true;
		}

		return false;
	}

	public virtual void Compute(Piece piece) {
		if (piece == null || piece.Node == null) return;
		BoundComputations(piece);
	}

	//returns true if met an ally or enemy, this is for square and triangle, to cause a block
	public bool ComputeMoveOrEatPieceEnemyAlly(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (toCheckNode.EmptySpace) {
			playerPiece.AddPossibleMoves(toCheckNode);
		} else if (Rules.IsEnemy(playerPiece, toCheckNode.Piece)) {
			AddToCheckOrEat(playerPiece, toCheckNode.Piece);
			return true;
		} else {
			return true;
		}

		return false;
	}

	private void AddToCheckOrEat(Piece playerPiece, Piece toCheckPiece) {
		if (Rules.CheckKing(playerPiece, toCheckPiece)) {
			playerPiece.Check = toCheckPiece;
		} else {
			playerPiece.AddPossibleEats(toCheckPiece.Node);
		}
	}
}
