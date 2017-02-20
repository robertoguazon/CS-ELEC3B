using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMovement :  Movement, IPieceMovement {

	public NoMovement() {
		BoundComputations += ComputeBound;
	}

	public void ComputeBound(Piece piece) {
		//do nothing
	}
}
