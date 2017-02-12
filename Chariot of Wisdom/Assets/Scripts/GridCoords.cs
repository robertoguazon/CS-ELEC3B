using UnityEngine;
using System.Collections;

[System.Serializable]
public struct GridCoords {
	public int row, col;

	public GridCoords(int row, int col) {
		this.row = row;
		this.col = col;
	}
}
