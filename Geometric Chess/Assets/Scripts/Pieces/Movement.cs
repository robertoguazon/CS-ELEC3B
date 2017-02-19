using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement{

	
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

	public void ComputePiece(GCPlayer player, Piece playerPiece, Node toCheckNode) {
		if (toCheckNode == null) return;
		if (toCheckNode.EmptySpace) {
			player.AddPossibleMoves(toCheckNode);

			toCheckNode.SetMaterial(GameManager.Instance.MoveHighlightMaterial);
		} else if (IsEnemy(playerPiece, toCheckNode.Piece)) {
			player.AddPossibleEats(toCheckNode);
			
			toCheckNode.SetMaterial(GameManager.Instance.EatHighlightMaterial);
		}
	}

	public virtual void Compute(Piece piece) {
		BoundComputations(piece);
	}
}
