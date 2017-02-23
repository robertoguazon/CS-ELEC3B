using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator {

	public static IPieceMovement CreatePieceMovement(MovementType movementType) {
		switch (movementType) {
			case MovementType.KING:
				return new KingMovement();
			case MovementType.PAWN:
				return new PawnMovement();
			case MovementType.ROOK:
				return new RookMovement();
			case MovementType.BISHOP:
				return new BishopMovement();
			case MovementType.QUEEN:
				return new QueenMovement();
			case MovementType.HORSE:
				return new HorseMovement();
			case MovementType.NONE:
			default:
				return new NoMovement();
		}
	}
}
