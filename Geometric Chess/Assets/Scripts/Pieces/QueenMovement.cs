using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMovement : Movement, IPieceMovement {

	private IPieceMovement rook;
	private IPieceMovement bishop;

	public QueenMovement() {
		rook = new RookMovement();
		bishop = new BishopMovement();
		BoundComputations += rook.ComputeBound;
		BoundComputations += bishop.ComputeBound;
	}

	public void ComputeBound(GCPlayer player, Piece piece) {
		//do nothing
	}

	public void Moved(Piece piece) {
		
	}
}
