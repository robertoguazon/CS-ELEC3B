using UnityEngine;
using System.Collections;

public class Grid {

	private int[,] grid;
	private int rows;
	private int cols;

	/*
	 * Constructor
	 * */
	public Grid(int sizeX, int sizeY){
		grid = new int[sizeX,sizeY];
		rows = sizeX;
		cols = sizeY;
	}

	/*
	 * Set value of grid[row,col] to num
	 * */
	public void setInt(int num, int row, int col){
		grid [row,col] = num;
	}

	/*
	 * return value of grid[row,col]
	 * */
	public int getInt(int row, int col){
		return grid [row,col];
	}

	/*
	 * check if cell has adjacent value of player
	 * */
	public bool hasAdjacent(int player, int row, int col){
			for (int i =row-1; i<row+2; i++) {
				for (int j = col-1;j<col+2; j++) {
				
				if (i < 0 || j < 0 || i > rows - 1 || j > cols - 1)
						continue;
				if (grid [i, j] == player) {
						return true;
					}

				}

			}
			return false;


	}

	/*
	 * compute for the winner
	 * */
	public int getWinner(){
		int player1C = 0, player2C = 0;
		int player1T1 = 0, player1T2 = 0;
		int player2T1 = 0, player2T2 = 0;

		//count all 3x3 diamond first
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				if (hasThreeThreeDiamond (1, i, j)) {
					player1T1++;
				}
				if (hasThreeThreeDiamond (2, i, j)) {
					player2T1++;
				}
			}
		}

		UIManagerScript.Instance.numPlayer1Triangle1 = player1T1;
		//UIManagerScript.Instance.numPlayer1Triangle2 = player1T2;
		UIManagerScript.Instance.numPlayer2Triangle1 = player2T1;
		//UIManagerScript.Instance.numPlayer2Triangle2 = player2T2;

		player1C = player1T1 * GameManagerScript.Instance.score2x3Triangle + player1T2 * GameManagerScript.Instance.score2x2Triangle;
		player2C = player2T1 * GameManagerScript.Instance.score2x3Triangle + player2T2 * GameManagerScript.Instance.score2x2Triangle;
		UIManagerScript.Instance.player1Count = player1C;
		UIManagerScript.Instance.player2Count = player2C;

		return (player1C > player2C) ? 1 : (player1C < player2C) ? 2 : 0;
	}

	/*
	* check if cell has 3x3 diamond
	* */
	bool hasThreeThreeDiamond(int player, int row, int col) {
		if (col - 1 < 0 || col + 1 >= cols) return false;
		if (row - 1 < 0 || row + 1 >= rows) return false;

		for (int r = -1; r < 1; r++) {
			for (int c = -1; c <= 1; c++) {
				if (Mathf.Abs(r) == Mathf.Abs(c)) continue;

				int checkRow = row + r;
				int checkCol = col + c;
				if (grid[checkRow, checkCol] != player) return false;
			}
		}

		//reset everything to 0 if no error
		for (int r = -1; r < 1; r++) {
			for (int c = -1; c <= 1; c++) {
				if (Mathf.Abs(r) == Mathf.Abs(c)) continue;

				int checkRow = row + r;
				int checkCol = col + c;
				grid[checkRow, checkCol] = 0;
			}
		}

		//if not did not return false
		return true;
	}

	/*
	 * check if cell has 2x3 triangle
	 * */
	bool hasTwoThreeTriangle(int player, int row, int col){
		if (col - 1 > -1 && col + 1 < cols && grid[row,col] == player && grid [row, col - 1] == player && grid [row, col + 1] == player) {
			if (row + 1 < rows && grid [row + 1, col] == player) {
				grid [row, col] = 0;
				grid [row, col + 1] = 0;
				grid [row, col - 1] = 0;
				grid [row + 1, col] = 0;
				return true;	
			} else if(row-1 > -1 && grid[row-1,col] == player){
				grid [row, col] = 0;
				grid [row, col+1] = 0;
				grid [row, col-1] = 0;
				grid [row - 1, col] = 0;
				return true;	
			}
		} 
		if (row - 1 > -1 && row + 1 < rows && grid[row,col] == player && grid [row - 1, col] == player && grid [row + 1, col] == player) {
			if (col + 1 < rows && grid [row, col + 1] == player) {
				grid [row, col] = 0;
				grid [row + 1, col ] = 0;
				grid [row - 1, col] = 0;
				grid [row , col+ 1] = 0;
				return true;	
			} else if(col - 1 > -1 && grid[row,col-1] == player){
				grid [row, col] = 0;
				grid [row + 1, col] = 0;
				grid [row - 1, col] = 0;
				grid [row, col - 1] = 0;
				return true;	
			}
		} 
		return false;
	}

	/*
	 * check if cell has 2x2 triangle
	 * */
	bool hasTwoTwoTriangle(int player, int row, int col){
		if (grid [row, col] == player) {
			if (row + 1 < rows && grid [row + 1, col] == player) {

				if (col - 1 > -1 && grid [row, col - 1] == player) {
					grid [row, col] = 0;
					grid [row, col - 1] = 0;
					grid [row + 1, col] = 0;
					return true;	
				} else if(col - 1 > -1 && grid[row+1,col-1] == player){
					grid [row, col] = 0;
					grid [row, col - 1] = 0;
					grid [row + 1, col - 1] = 0;
					return true;
				} else if (col + 1 > cols && grid [row, col + 1] == player) {
					grid [row, col] = 0;
					grid [row, col + 1] = 0;
					grid [row + 1, col] = 0;
					return true;	
				} else if(col + 1 > cols && grid[row+1,col+1] == player){
					grid [row, col] = 0;
					grid [row, col + 1] = 0;
					grid [row + 1, col + 1] = 0;
					return true;
				}
					
			} 
			if (row - 1 > -1 && grid [row - 1, col] == player) {
				if (col - 1 > -1 && grid [row, col - 1] == player) {
						grid [row, col] = 0;
						grid [row, col - 1] = 0;
						grid [row - 1, col] = 0;
						return true;	
				} else if (col - 1 > -1 && grid [row - 1, col - 1] == player) {
						grid [row, col] = 0;
						grid [row, col - 1] = 0;
						grid [row - 1, col - 1] = 0;
						return true;
				} else if (col + 1 > cols && grid [row, col + 1] == player) {
					grid [row, col] = 0;
					grid [row, col + 1] = 0;
					grid [row - 1, col] = 0;
					return true;	
				} else if (col + 1 > cols && grid [row - 1, col + 1] == player) {
					grid [row, col] = 0;
					grid [row, col - 1] = 0;
					grid [row - 1, col - 1] = 0;
					return true;
				}
			}

		} 
		return false;
	}

	/*
	 * check if there exists a cell for the player to choose
	 * */
	public bool canPlay(int player){
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				if (grid[i,j] == 0 && hasAdjacent (player, i, j)) {
					return true;
				}
			}
		}
		return false;
	}
}
