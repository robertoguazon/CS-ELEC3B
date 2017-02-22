using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator {

	public static IPieceMovement CreatePieceMovement(MovementType movementType) {
		switch (movementType) {
			case MovementType.CROSS:
				return new CrossMovement();
			case MovementType.CIRCLE:
				return new CircleMovement();
			case MovementType.SQUARE:
				return new SquareMovement();
			case MovementType.TRIANGLE:
				return new TriangleMovement();
			case MovementType.HEXAGON:
				return new HexagonMovement();
			case MovementType.NONE:
			default:
				return new NoMovement();
		}
	}
}
