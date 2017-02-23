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
	HORSE,
}

public abstract class Movement : ScriptableObject {

	
	public event ComputeBound BoundComputations;

	public bool IsAlly(Piece piece1, Piece piece2) {
		GCPlayer p1 = GameManager.Instance.P1;
		if (p1.Has(piece1) && p1.Has(piece2)) return true;
		
		GCPlayer p2 = GameManager.Instance.P2;
		if (p2.Has(piece1) && p2.Has(piece2)) return true;

		return false;
	}

	public bool IsEnemy(Piece piece1, Piece piece2) {
		GCPlayer p1 = GameManager.Instance.P1;
		if (p1.Has(piece1) && !p1.Has(piece2)) return true;
		if (p1.Has(piece2) && !p1.Has(piece1)) return true;
		return false;
	}

	public bool ComputeMovePiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (toCheckNode.EmptySpace) {
			player.AddPossibleMoves(toCheckNode);
			toCheckNode.MoveHighlight();
			return true;
		}

		return false;
	}

	public bool ComputeEatPiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (!toCheckNode.EmptySpace && IsEnemy(playerPiece, toCheckNode.Piece)) {
			player.AddPossibleEats(toCheckNode);
			toCheckNode.EatHighlight();
			return true;
		}

		return false;
	}

	public bool ComputeMoveOrEatPiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (toCheckNode.EmptySpace) {
			player.AddPossibleMoves(toCheckNode);

			toCheckNode.MoveHighlight();
			return true;
		} else if (IsEnemy(playerPiece, toCheckNode.Piece)) {
			player.AddPossibleEats(toCheckNode);
			
			toCheckNode.EatHighlight();
			return true;
		}

		return false;
	}

	public virtual void Compute(Piece piece) {
		BoundComputations(piece);
	}

	//returns true if met an ally or enemy, this is for square and triangle, to cause a block
	public bool ComputeMoveOrEatPieceEnemyAlly(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return false;
		if (toCheckNode.EmptySpace) {
			player.AddPossibleMoves(toCheckNode);
			toCheckNode.MoveHighlight();
		} else if (IsEnemy(playerPiece, toCheckNode.Piece)) {
			player.AddPossibleEats(toCheckNode);
			toCheckNode.EatHighlight();
			return true;
		} else {
			return true;
		}

		return false;
	}
}
