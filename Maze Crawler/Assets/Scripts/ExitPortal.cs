using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPortal : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
		text.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Tank") {
			if (MyGameManager.Instance.CheckWin()) {
				text.color = Color.green;
				PlayerPrefs.SetInt(GameOverManager.GAME_OVER, GameOverManager.WIN);
				MyGameManager.Instance.LoadNextLevel();
			} else {
				HUDManager.Instance.DisplayTextFor("Kill all the enemies first",2f);
			}
		}
	}
}
