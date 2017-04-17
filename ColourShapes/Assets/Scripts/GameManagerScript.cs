/*
 * GameManagerScript 
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript Instance;

	public Text redGameScore;
	public Text blueGameScore;

	public GameObject replayButton;
	public GameObject quitButton;
	public GameObject winnerCanvas;

	public GameObject chip;
	public GameObject[] chips;

	public int gridSize=15;
	public int fillPlayer = 0;
	public int score2x3Triangle = 12;
	public int score2x2Triangle = 6;

	//events
	public delegate void EventDelegate();
	public static event EventDelegate GameOverEvent;

	[HideInInspector] public bool isPaused = false;

	private Grid grid;
	private int numOfChips = 0;
	private bool isPlayerOne = true;
	private bool player1FirstTurn = true;
	private bool player2FirstTurn = true;

	public static GameManagerScript instance;

	public GameManagerScript() {
		instance = this;
	}

	void Awake(){
		LoadGameScores();
		GameOverEvent += onGameOver;
	}

	void Start () {
		grid = new Grid (gridSize, gridSize);
		if (Instance == null)
			Instance = this;
		//instaWin ();

	}

	void LoadGameScores() {
		redGameScore.text = PlayerPrefs.GetInt(GameManagerScript.PLAYER_RED,0).ToString();
		blueGameScore.text = PlayerPrefs.GetInt(GameManagerScript.PLAYER_BLUE,0).ToString();
	}

	void Update () {

#if UNITY_EDITOR
		if (Input.GetKeyUp(KeyCode.Q)) {	//press Q to fill all remaining with player1
			fillPlayer = 1;
			Instance.fillEmptyCells ();
		} else if (Input.GetKeyUp(KeyCode.W)) { // press W to fill all remaining with player2
			fillPlayer = 2;
			Instance.fillEmptyCells ();
		} else if (Input.GetKeyUp(KeyCode.Space)) { //press space to change player
			Instance.changePlayer();
		}
#endif
		
		//checks on mouse click
		if (Input.GetMouseButtonDown (0) && !isPaused) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D rayHit = Physics2D.Raycast(ray.origin,ray.direction,Mathf.Infinity);
			GameObject clicked;
			if (rayHit) {
				clicked = rayHit.collider.gameObject;
				int i = System.Array.IndexOf (chips, clicked);
				if (i > -1) {
					int row = (int)(i / gridSize);
					int col = i % gridSize;
					if (grid.getInt (row, col) == 0) {
						bool canPlay = (isPlayerOne) ? grid.hasAdjacent (1, row, col) : grid.hasAdjacent (2, row, col);
						canPlay = canPlay || ((isPlayerOne)?player1FirstTurn:player2FirstTurn);

						//check if clicked is a grid cell and cell has adjacent chip of player
						if (clicked.tag == "Chip" && clicked.GetComponent<SpriteRenderer> ().enabled == false && canPlay) {
							ShowChip (clicked);
							numOfChips++;
							changePlayer ();
						}

						//check if player 1 has possible moves
						if (!player1FirstTurn && !grid.canPlay (1)) {
							Debug.Log ("Player 1 stuck!");
							isPaused = true;
							fillPlayer = 2;
							UIManagerScript.Instance.CallPlayerStuck (1);
						}

						//check if player 2 has possible moves
						if (!player2FirstTurn && !grid.canPlay (2)) {
							isPaused = true;
							Debug.Log ("Player 2 stuck!");
							fillPlayer = 1;
							UIManagerScript.Instance.CallPlayerStuck (2);
						}
					}

				}

			}
		}
		if (numOfChips == gridSize * gridSize) {
			isPaused = true;
			if (GameOverEvent != null) {
				GameOverEvent ();
				GameOverEvent -= onGameOver;
			}

		}

	}

	/*
		displays chip on board
	*/
	void ShowChip(GameObject clicked){
		int i = System.Array.IndexOf (chips, clicked);
		if (i > -1) {
			int row = (int)(i / gridSize);
			int col = i % gridSize;
			grid.setInt((isPlayerOne)? 1:2,row,col); //fills the grid with integer values for checking after the game

			if (isPlayerOne) {
				clicked.GetComponent<ChipScript> ().ShowChip("red");
				UIManagerScript.Instance.ChangePlayerTurn (2);
				player1FirstTurn = false;
			} else {
				clicked.GetComponent<ChipScript> ().ShowChip("blue");
				UIManagerScript.Instance.ChangePlayerTurn (1);
				player2FirstTurn = false;
			}

		}

	}

	/*
		change player
	*/
	public void changePlayer(){
		isPlayerOne = !isPlayerOne;
		if(isPlayerOne) UIManagerScript.Instance.ChangePlayerTurn (1);
		else UIManagerScript.Instance.ChangePlayerTurn (2);
	}

	/*
		when game ends
	*/
	public void onGameOver(){
		int winner = grid.getWinner ();
		UIManagerScript.Instance.CallGameOver(winner);
	}

	/*
		FOR DEBUGGING PURPOSES ONLY
		fill grid with RED chips (not visible)
	*/
	void instaWin(){
		for (int i = 0; i < gridSize; i++) {
			for (int j = 0; j < gridSize; j++) {
				grid.setInt(1,i,j);
				numOfChips++;
			}
		}
	}

	/*
		fill all empty cells with fillPlayer value
	*/
	public void fillEmptyCells(){
		if (fillPlayer == 1)
			isPlayerOne = true;
		else
			isPlayerOne = false;
		for (int i = 0; i < gridSize; i++) {
			for (int j = 0; j < gridSize; j++) {
				if (grid.getInt (i, j) == 0) {
					ShowChip (chips [i * gridSize + j]);
				}
			}
		}
		numOfChips = gridSize  *gridSize;
		UIManagerScript.Instance.DisablePlayerStuckMenu ();
		isPaused = false;
	}

	public const string PLAYER_RED = "player_red";
	public const string PLAYER_BLUE = "player_blue";
	public const string GAME_MAX_SCORE = "game_max_score";
	public const string GAME_MAX_GAMES = "game_max_games";
	public const string GAME_CURRENT_GAMES = "game_current_games";

	public static void AddGame() {
		int maxGames = PlayerPrefs.GetInt(GAME_MAX_GAMES,0);
		int currentGames = PlayerPrefs.GetInt(GAME_CURRENT_GAMES,0);
		PlayerPrefs.SetInt(GAME_CURRENT_GAMES,currentGames + 1);

		if (currentGames >= maxGames) {
			WinGame();
		}
	}

	public static void AddScore(string playerString) {
		int newScore = PlayerPrefs.GetInt(playerString,0)+ 1;
		PlayerPrefs.SetInt(playerString,newScore);
		int maxScore = PlayerPrefs.GetInt(GAME_MAX_SCORE,0);
		if (newScore == maxScore) {
			WinGame();
		}
	}

	public static void ResetPlayersScore() {
		PlayerPrefs.DeleteKey(PLAYER_RED);
		PlayerPrefs.DeleteKey(PLAYER_BLUE);
	}

	static void WinGame() {
		if (instance != null) {
			instance.winnerCanvas.SetActive(true);
			instance.replayButton.SetActive(false);
			instance.quitButton.SetActive(false);
		}
	}
}
