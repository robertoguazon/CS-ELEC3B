using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;
public class TargetLock : MonoBehaviour {

	private Sparty target;
	private int indexTarget;

	private TankShooting shooting;

	// Use this for initialization
	void Start () {
		indexTarget = -1;
		shooting = GetComponent<TankShooting>();
		SetTarget();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) { // change target
			SetTarget();
		}
	}

	public void SetTarget() {
		if (target != null) {
			target.DeactivateHealthBar();
		}

		List<Sparty> spartyClones = MyGameManager.Instance.SpartyClones;
		if (spartyClones == null) return;
		if (++indexTarget >= spartyClones.Count) {
			indexTarget = 0;
		}
		if (spartyClones.Count == 0) return;
		target = spartyClones[indexTarget];
		shooting.target = target.gameObject;
		target.ActivateHealthBar();
	}

	public void ReTarget() {
		SetTarget();
	}
	
}
