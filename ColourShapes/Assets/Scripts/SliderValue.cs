using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {

	public Slider sliderGame;
	public Text sliderGameText;

	public Slider sliderScore;
	public Text sliderScoreText;

	void Awake() {
		int oldMaxScore = PlayerPrefs.GetInt(GameManagerScript.GAME_MAX_SCORE,5);
		int oldMaxGame = PlayerPrefs.GetInt(GameManagerScript.GAME_MAX_GAMES,10);
		sliderScore.value = oldMaxScore;
		sliderScoreText.text = oldMaxScore.ToString();

		sliderGame.value = oldMaxGame;
		sliderGameText.text = oldMaxGame.ToString();
	}

	public void OnGameValueChanged() {
		int sliderValue = (int)sliderGame.value;
		PlayerPrefs.SetInt(GameManagerScript.GAME_MAX_GAMES, sliderValue);
		GameManagerScript.ResetPlayersScore();
		sliderGameText.text = sliderValue.ToString();
 	}

	public void OnScoreValueChanged() {
		int sliderValue = (int)sliderScore.value;
		PlayerPrefs.SetInt(GameManagerScript.GAME_MAX_SCORE, sliderValue);
		GameManagerScript.ResetPlayersScore();
		sliderScoreText.text = sliderValue.ToString();
 	}

	
}
