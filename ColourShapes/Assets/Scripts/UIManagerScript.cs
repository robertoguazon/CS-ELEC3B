using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour {

	public static UIManagerScript Instance;

	public GameObject pauseMenu;
	public GameObject pauseButton;
	public GameObject resumeButton;
	public GameObject replayButton;
	public GameObject quitButton;
	public GameObject playerLabel;
	public GameObject gameOverMenu;
	public GameObject winnerLabel;
	public GameObject playerStuckLabel;
	public GameObject playerStuckMenu;
	public GameObject player1Label;
	public GameObject player2Label;
	public GameObject player1Triangle1;
	public GameObject player1Triangle2;
	public GameObject player2Triangle1;
	public GameObject player2Triangle2;

	[HideInInspector] public int playerTurn;

	public int numPlayer1Triangle1 = 0;
	public int numPlayer1Triangle2 = 0;
	public int numPlayer2Triangle1 = 0;
	public int numPlayer2Triangle2 = 0;
	public int player1Count = 0;
	public int player2Count = 0;

	void Start () {
		if (Instance == null)
			Instance = this;
	}

	/*
	 * show Game Over Menu
	 * */
	public void CallGameOver(int playerWon){
		if (playerWon != 0) {
			winnerLabel.GetComponent<Text> ().text = "Player " + playerWon + " won!";
		} else {
			winnerLabel.GetComponent<Text> ().text = "IT'S A TIE!";
		}

		player1Label.GetComponent<Text> ().text = "Player 1 Score: " + player1Count;
		player2Label.GetComponent<Text> ().text = "Player 2 Score: " + player2Count;

		player1Triangle1.GetComponent<Text> ().text = "3 x 3 Diamonds: " + numPlayer1Triangle1; 
		player2Triangle1.GetComponent<Text> ().text = "3 x 3 Diamonds: " + numPlayer2Triangle1; 
		player1Triangle2.GetComponent<Text> ().text = "";
		player2Triangle2.GetComponent<Text> ().text = "";

		gameOverMenu.GetComponent<CanvasRenderer> ().SetAlpha (50);
		gameOverMenu.SetActive (true);
	}

	/*
	 * Change player turn in UI
	 * */
	public void ChangePlayerTurn(int playerTurn){
		playerLabel.GetComponent<Text> ().color = (playerTurn == 1) ? Color.red : Color.blue;
		playerLabel.GetComponent<Text> ().text = ""+playerTurn;
	}

	/*
	 * show Player Stuck PopUp/Menu
	 * */
	public void CallPlayerStuck(int playerStuck){
		playerStuckLabel.GetComponent<Text> ().text = "PLAYER " + playerStuck;
		playerStuckLabel.GetComponent<Text> ().color = (playerStuck == 1) ? Color.red : Color.blue;
		playerStuckMenu.GetComponent<CanvasRenderer> ().SetAlpha (200);
		playerStuckMenu.SetActive (true);
	}
		
	public void DisablePlayerStuckMenu(){
		playerStuckMenu.SetActive (false);
	}

	/*
	 * Pause the game and call Pause Menu
	 * */
	public void OnPauseClick(){
		GameManagerScript.Instance.isPaused = true;
		pauseMenu.GetComponent<CanvasRenderer> ().SetAlpha (200);
		pauseMenu.SetActive (true);

	}

	/*
	 * Close the pause menu and resume the game
	 * */
	public void OnResumeClick(){
		GameManagerScript.Instance.isPaused = false;
		pauseMenu.SetActive (false);

	}

	/*
	 * Reload the level
	 * */
	public void OnReplayClick(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	/*
	 * Return to main menu
	 * */
	public void OnQuitClick(){
		SceneManager.LoadScene ("MainMenu");
	}
}
