using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComputeBound(GCPlayer player, Piece piece);

public interface IPieceMovement {

	event ComputeBound BoundComputations;
	void ComputeBound(GCPlayer player, Piece piece);
	void Compute(GCPlayer player, Piece piece);
}
