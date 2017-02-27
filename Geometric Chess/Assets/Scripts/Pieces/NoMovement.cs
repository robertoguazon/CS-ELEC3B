using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMovement :  Movement, IPieceMovement {

	public NoMovement(GCPlayer player, Piece piece) : base(player,piece) {
		BoundComputations += ComputeBound;
	}

	public void ComputeBound() {
		//do nothing
	}

	public void Moved() {
		
	}
}
