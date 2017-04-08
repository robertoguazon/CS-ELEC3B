using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		if (MyGameManager.Instance == null) return;
		PlayerPrefs.SetInt(GameOverManager.GAME_OVER, GameOverManager.LOSE);
		MyGameManager.Instance.LoadScene("GameOver", 2f);
	}
}
