using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Complete;

public class MyGameManager : Singleton<MyGameManager>  {
	
	public GameObject spartyContainer;
	public GameObject tank;
	public CameraControl m_CameraControl;

	public string nextLevelName;
	public float winDelaySeconds = 3f;
	public float waitStartSeconds = 3f;

	private List<Sparty> spartyClones;

	public List<Sparty> SpartyClones {
		get {return spartyClones;}
	}

	public bool SpartyAreDead {
		get {
			return spartyClones.Count == 0;
		}
	}

	public bool IsTankAlive {
		get {
			return tank != null;
		}
	}

	void Awake() {
		PlayerPrefs.DeleteKey(GameOverManager.GAME_OVER);
		spartyClones = new List<Sparty>();
		
		int children = spartyContainer.transform.childCount;
		for (int i = 0; i < children; i++) {
			GameObject go = spartyContainer.transform.GetChild(i).gameObject;
			Sparty sparty = go.GetComponent<Sparty>();
			if (sparty != null) {
				spartyClones.Add(sparty);
				sparty.target = tank;
			}
		}

		SetCameraTargets();
	}

	void Start() {
		HUDManager.Instance.DisplayTextFor("KILL ALL THE ENEMIES AND FIND THE EXIT", 5f);
	}

	public void RemoveSparty(Sparty sparty) {
		spartyClones.Remove(sparty);
		if (tank != null) {
			tank.GetComponent<TargetLock>().ReTarget();
		}
		//CheckWin();
	}

	void Update() {
		
	}

	public bool CheckWin() {
		if (SpartyAreDead) {
			return true;
		}

		return false;
	}

	public void LoadNextLevel() {
		LoadScene(nextLevelName,winDelaySeconds);
	}

	public void LoadScene(string levelName) {
		StartCoroutine(IELoadScene(levelName, 0));
	}

	public void LoadScene(string levelName, float waitFor) {
		StartCoroutine(IELoadScene(levelName, waitFor));
	}

	IEnumerator IELoadScene(string levelName, float waitFor) {
		yield return new WaitForSeconds(waitFor);
		SceneManager.LoadScene(levelName);
	}

	private void SetCameraTargets() {
        
		// Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[spartyClones.Count + 1];

        targets[0] = tank.transform;
		int startIndex = 1;
		for (int i = 0; i < spartyClones.Count; i++) {
			targets[startIndex + i] = spartyClones[i].transform;
		}

        // These are the targets the camera should follow.
    	m_CameraControl.m_Targets = targets;
    }
}