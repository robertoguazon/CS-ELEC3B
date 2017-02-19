using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComputeBound(Piece piece);

public interface IPieceMovement {

	event ComputeBound BoundComputations;
	void ComputeBound(Piece piece);
	void Compute(Piece piece);
}
