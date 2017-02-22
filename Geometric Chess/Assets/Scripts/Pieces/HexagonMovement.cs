using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMovement : Movement, IPieceMovement {

	private IPieceMovement square;
	private IPieceMovement triangle;

	public HexagonMovement() {
		square = new SquareMovement();
		triangle = new TriangleMovement();
		BoundComputations += square.ComputeBound;
		BoundComputations += triangle.ComputeBound;
	}

	public void ComputeBound(Piece piece) {
		//do nothing
	}
}
