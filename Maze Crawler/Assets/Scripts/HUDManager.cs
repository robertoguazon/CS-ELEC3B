using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager> {

	public GameObject displayContainer;
	public Text displayText;

	void Awake() {
		HideText();
	}

	// Use this for initialization
	void Start () {
	}

	public void DisplayText(string text) {
		displayText.text = text;
		displayContainer.SetActive(true);
	}

	public void HideText() {
		displayContainer.SetActive(false);
	}

	public void DisplayTextFor(string text, float sec) {
		StartCoroutine(IEDisplayTextFor(text,sec));
	}

 	IEnumerator IEDisplayTextFor(string text, float sec) {
		DisplayText(text);
		yield return new WaitForSeconds(sec);
		HideText();
	}
}
