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

		if (player1C > player2C) {
			GameManagerScript.AddScore(GameManagerScript.PLAYER_RED);
		} else if (player1C < player2C) {
			GameManagerScript.AddScore(GameManagerScript.PLAYER_BLUE);
		}

		GameManagerScript.AddGame();

		return (player1C > player2C) ? 1 : (player1C < player2C) ? 2 : 0;
	}

	 Mesh CreateMesh(float width, float height) {

		if (GameManagerScript.Instance.gridSize == 15) {
			width -= 0.5f;
			height -= 0.5f;
		}
	

		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = new Vector3[] {
			new Vector3(0, -height, 0.01f),
			new Vector3(width, 0, 0.01f),
			new Vector3(0, height, 0.01f),
			new Vector3(-width, 0, 0.01f)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0.5f, 0),
			new Vector2(0, 0.5f),
			new Vector2 (-0.5f, 0)
		};
		m.triangles = new int[] {0,2,3,0,1,2};
		m.RecalculateNormals();
			
		return m;
	}

	void DrawMesh(Vector3 position, int player) {
		Color color = Color.green; //default

		//set to the following players' color
		if (player == 1) {
			color = new Color(0.5f,0,0);
		} else {
			color = new Color(0,0,0.5f);
		}

		GameObject plane = new GameObject("Plane");
		MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = CreateMesh(1.5f, 1.5f);
		MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.sortingLayerName = "GUI";
		renderer.material.shader = Shader.Find ("Sprites/Default");
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(0, 0, color);
		tex.Apply();
		renderer.material.mainTexture = tex;
		renderer.material.color = color;
		plane.transform.position = position;
	}

	void DrawLines(int[,] points, int player) {

		//create a new gameobject to add a linerenderer component to
		GameObject checker = new GameObject("lines");

		//create the line renderer to the created game object
		LineRenderer lr = checker.AddComponent<LineRenderer>();

		//set the material of the line renderer
		//remember to add the material used in edit->project settings->graphics->always included shaders
		lr.material = new Material(Shader.Find("Sprites/Default"));

		//default green color when no player parameter matched
		Color color = Color.green; //default

		//set to the following players' color
		if (player == 1) {
			color = new Color(0.5f,0,0);
		} else {
			color = new Color(0,0,0.5f);
		}

		//set the start and end of gradient of line renderer to the color
		lr.startColor = color;
		lr.endColor = color;

		//remeber to make a sorting layer named GUI
		lr.sortingLayerName = "GUI";

		//set the start and end width
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;

		//get the length of the 1st dimension of the points array
		int length = points.GetLength(0);

		//set the number of points (corners) including the first(+1)to connect the first and last
		lr.numPositions = 8 + 1;

		//index will be used for placing points on the linerenderer for sorting purposes
		int index = 0;

		//draw on corners of top chip
		index = AddToLineRenderer(lr,index,points[0,0],points[0,1],2,2,false);

		//draw on corners of left chip
		index = AddToLineRenderer(lr,index,points[1,0],points[1,1],2,3,false);

		//draw on corners of bottom chip
		index = AddToLineRenderer(lr,index,points[3,0],points[3,1],2,4,false);

		//draw on corners of right chip
		index = AddToLineRenderer(lr,index,points[2,0],points[2,1],2,1,false);

		//connect to last
		index = AddToLineRenderer(lr, index,points[0,0],points[0,1],2);
	}

	//add a point to linerenderer based on a certain corner of the chip
	//at - where to put the point, what corner?
	int AddToLineRenderer(LineRenderer lr, int index, int row, int col, int at) {

		//get the gameobject of the chip base on row,col
		GameObject curr = GameManagerScript.Instance.chips[row * cols + col];

		//get the size of the sprite used
		Vector3 chipSize = curr.GetComponent<SpriteRenderer>().sprite.bounds.size;	
		
		//adjust size depending whether on 9x9 or 15x15 since the corners are bigger on 15x15
		if (GameManagerScript.Instance.gridSize == 9) {
			chipSize.x -= 0.25f;
			chipSize.y -= 0.25f;
		} else if (GameManagerScript.Instance.gridSize == 15) {
			chipSize.x -= 0.45f;
			chipSize.y -= 0.45f;
		}
		

		//halfSize of the chip, used for computing the corners
		float xHalf = chipSize.x / 2;
		float yHalf = chipSize.y / 2;

		//Find the corners or the points so it will be easy to draw lines later
		Vector3 bottomLeft, bottomRight, topRight, topLeft;
		bottomLeft = bottomRight = topRight = topLeft = curr.transform.position;
		
		//computation of corners for a certain chip
		topRight.y = topLeft.y += yHalf;
		bottomLeft.x = topLeft.x -= xHalf;

		bottomRight.y = bottomLeft.y -= yHalf;
		topRight.x = bottomRight.x += xHalf; 

		//place point at position -> used by line renderer to draw lines based on points
		switch (at) {
			case 1: //bottomRight corner
				lr.SetPosition(index++, bottomRight);
			break; 
			case 2: //topRight corner
				lr.SetPosition(index++, topRight);
			break;
			case 3: //topLeft corner
				lr.SetPosition(index++, topLeft);
			break;
			case 4: //bottomLeft corner
				lr.SetPosition(index++, bottomLeft);
			break;
		}

		//return the index for keeping track of the lineRenderer's position so that it does not exceed size;
		return index;
	}

	//startAt - what corner to start
	//clockwise - from the start corner add point clockwise or counter clockwise
	//number -> number of corners that will be used
	int AddToLineRenderer(LineRenderer lr, int index, int row, int col, int number, int startAt, bool clockwise) {

		//use for placing points to linerenderer 
		int corner = 0;

		//used for clockwise or not
		int add;
		if (clockwise) {
			add = -1;
		} else {
			add = 1;
		}

		switch (startAt) {
			case 1: //bottomRight corner first; possible: 1234, 1432
				corner = 1;
			break;
			case 2: //topRight corner first; possible: 2341, 2143
				corner = 2;
			break;
			case 3: //topLeft corner first; possible: 3412, 3214
				corner = 3;
			break;
			case 4: //bottomLeft corner first; possible: 4123, 4321
				corner = 4;
			break;
		}

		//put points
		for (int i = 0; i < number; i++) {
			index = AddToLineRenderer(lr,index,row,col,corner);
			Debug.Log(corner);
			corner += add;

			//if exceeds then loop
			if (corner < 1) corner = 4;
			if (corner > 4) corner = 1;
		}

		return index;
	}

	/*
	* check if cell has 3x3 diamond
	* */
	bool hasThreeThreeDiamond(int player, int row, int col) {
		if (col - 1 < 0 || col + 1 >= cols) return false;
		if (row - 1 < 0 || row + 1 >= rows) return false;
		if (grid[row,col] != player) return false;

		//array for storing the cell(row,col) of the chips on the corner
		int [,] points = new int[5,2];
		int pCounter = 0;

		//check if has diamond
		for (int r = -1; r <= 1; r++) {
			for (int c = -1; c <= 1; c++) {
				if (Mathf.Abs(r) == Mathf.Abs(c)) continue;
				
				int checkRow = row + r;
				int checkCol = col + c;

				//return and skip checking all other sides of the center chip if 1 side failed
				if (grid[checkRow, checkCol] != player) return false;
				else {

					//if match with player chip then add to possible points 
					points[pCounter,0] = checkRow;
					points[pCounter,1] = checkCol;
					pCounter++;
				}
			}
		}

		//reset everything to 0 if no error
		for (int r = -1; r <= 1; r++) {
			for (int c = -1; c <= 1; c++) {
				if (Mathf.Abs(r) == Mathf.Abs(c)) continue;

				int checkRow = row + r;
				int checkCol = col + c;
				grid[checkRow, checkCol] = 0;
			}
		}
		grid[row,col] = 0;


		//draw lines and pass the cells of the chip
		//DrawLines(points,player);
		GameObject curr = GameManagerScript.Instance.chips[row * cols + col];
		DrawMesh(curr.transform.position,player);
		//if not did not return false
		return true;
	}

	/*
	* check if cell has 5x5 diamond
	* */
	bool hasFiveFiveDiamond(int player, int row, int col) {
		if (col - 1 < 0 || col + 1 >= cols) return false;
		if (row - 1 < 0 || row + 1 >= rows) return false;
		if (row - 2 < 0 || row + 2 >= rows) return false;
		if (col - 2 < 0 || col + 2 >= cols) return false;

		//check all nearby +1s and -1s
		for (int r = -1; r <= 1; r++) {
			for (int c = -1; c <= 1; c++) {
				int checkRow = row + r;
				int checkCol = col + c;
				if (grid[checkRow, checkCol] != player) return false;
			}
		}
		
		//check all far  up, down, left,right
		if (grid[row - 2, col] != player) return false;
		if (grid[row + 2, col] != player) return false;
		if (grid[row, col + 2] != player) return false;
		if (grid[row, col + -2] != player) return false;

		//reset everything to 0 if no error
		for (int r = -1; r <= 1; r++) {
			for (int c = -1; c <= 1; c++) {

				int checkRow = row + r;
				int checkCol = col + c;
				grid[checkRow, checkCol] = 0;
			}
		}

		//reset all far  up, down, left,right
		grid[row - 2, col] = 0;
		grid[row + 2, col] = 0;
		grid[row, col + 2] = 0;
		grid[row, col + -2] = 0;

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
