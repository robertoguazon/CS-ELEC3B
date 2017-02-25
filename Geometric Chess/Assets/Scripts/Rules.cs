using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour {

	public static bool IsAlly(Piece piece1, Piece piece2) {
		GCPlayer p1 = GameManager.Instance.P1;
		if (p1.Has(piece1) && p1.Has(piece2)) return true;
		
		GCPlayer p2 = GameManager.Instance.P2;
		if (p2.Has(piece1) && p2.Has(piece2)) return true;

		return false;
	}

	public static bool IsEnemy(Piece piece1, Piece piece2) {
		GCPlayer p1 = GameManager.Instance.P1;
		if (p1.Has(piece1) && !p1.Has(piece2)) return true;
		if (p1.Has(piece2) && !p1.Has(piece1)) return true;
		return false;
	}

	public static bool CheckKing(Piece checkedBy, Piece checkedPiece) {
		if (checkedPiece.PieceType == PieceType.CROSS) {
			GameManager.Instance.CurrentPlayer.CheckedBy = checkedBy;
			//checkedPiece.Node.HighlightCheck(); //Experimental
			//checkedBy.Node.HighlightCheck(); //Experimental
			Debug.Log("Checked By: " + checkedBy);
			return true;
		}
		return false;
	}
}
