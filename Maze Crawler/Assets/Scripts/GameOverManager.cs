using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	public Text winloseText;
	private bool win = false;

	public const int LOSE = 0;
	public const int WIN = 1;
	public const string GAME_OVER = "gameOver";

	void Awake() {
	
	}

	// Use this for initialization
	void Start () {
		int gameOver = PlayerPrefs.GetInt(GAME_OVER);
		Debug.Log("GameOver: " + gameOver);
		if (gameOver == WIN) {
			win = true;
			winloseText.text = "WIN";
		} else {
			winloseText.text = "LOSE";
			win = false;
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
