using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	public Camera mainCamera;
	public List<GameObject> coins;
	public GameObject apple;
	public int coinPoints = 5;

	public Text coinText;
	public Text pointsText;
	public bool loadCoinsFromPref = false;
	public bool loadPointsFromPref = false;

	public string nextLevel;
	public float delayBeforeNextLevel = 1f;

	private int guiCoins = 0;
	private int guiPoints = 0;

	public Camera MainCamera {
		get {
			if (mainCamera == null) {
				mainCamera = Camera.main;
			}

			return mainCamera;
		}
	}

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		if (loadCoinsFromPref) {
			GetCoinsFromPref();
		} else {
			ResetCoinsFromPref();
		}

		if (loadPointsFromPref) {
			GetPointsFromPref();
		} else {
			ResetCoinsFromPref();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ResetPointsFromPref() {
		PlayerPrefs.DeleteKey("points");
	}

	public void GetPointsFromPref() {
		guiPoints = PlayerPrefs.GetInt("points");
		UpdateGUIPoints();
	}

	public void UpdateGUIPoints() {
		if (pointsText == null) return;
		pointsText.text = guiPoints.ToString();
	}

	public void SavePointsToPref() {
		PlayerPrefs.SetInt("points", guiPoints);
	}

	public void ResetCoinsFromPref() {
		PlayerPrefs.DeleteKey("coins");
	}

	public void GetCoinsFromPref() {
		guiCoins = PlayerPrefs.GetInt("coins");
		UpdateGUICoins();
	}

	public void SaveCoinsToPref() {
		PlayerPrefs.SetInt("coins", guiCoins);
	}

	public bool CheckApple(GameObject apple) {
		if (apple == null) return false;
		if (this.apple == apple) {
			NextLevel(nextLevel,delayBeforeNextLevel);

			return true;
		}

		return false;
	}

	public void IncrementGUICoins() {
		guiCoins++;
		guiPoints += coinPoints;
		UpdateGUICoins();
		UpdateGUIPoints();
	}

	public void UpdateGUICoins() {
		if (coinText == null) return;
		coinText.text = guiCoins.ToString();
	}

	public void GrabCoin(GameObject coin) {
		IncrementGUICoins();
		coins.Remove(coin);
	}

	public void NextLevel(string nextLevel) {
		StartCoroutine(IENextLevel(nextLevel,0));
	}

	public void NextLevel(string nextLevel, float waitFor) {
		StartCoroutine(IENextLevel(nextLevel,waitFor));
	}

	 IEnumerator IENextLevel(string nextLevel, float waitFor) {
		yield return new WaitForSeconds(waitFor);
		SaveCoinsToPref();
		SavePointsToPref();
		SceneManager.LoadScene(nextLevel);
	}
}
