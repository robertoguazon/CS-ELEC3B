using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public string loadOnPlay = "level1";

	public void Quit() {
		Application.Quit();
	}

	public void Load(string scene) {
		SceneManager.LoadScene(scene);
	}


}
