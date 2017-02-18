using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelPickerScript : MonoBehaviour {

	public void OnLevel1Click(){
		SceneManager.LoadScene ("Level1");
	}
	public void OnLevel2Click(){
		SceneManager.LoadScene ("Level2");
	}
}
