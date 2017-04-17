using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerManager : MonoBehaviour {

	public Text redGameScore;
	public Text blueGameScore;
	public Text winnerScore;

	void Awake() {
		LoadGameScores();
		GameManagerScript.ResetPlayersScore();
	}

	void LoadGameScores() {
		int red = PlayerPrefs.GetInt(GameManagerScript.PLAYER_RED,0);
		int blue = PlayerPrefs.GetInt(GameManagerScript.PLAYER_BLUE,0);
		redGameScore.text = red.ToString();
		blueGameScore.text = blue.ToString();

		if (red > blue) {
			winnerScore.text = "PLAYER RED WINS THE GAME";
		} else if (red < blue) {
			winnerScore.text = "PLAYER BLUE WINS THE GAME";
		} else {
			winnerScore.text = "IT'S A TIE";
		}
	}
}
