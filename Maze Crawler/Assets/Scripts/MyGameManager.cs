using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

public class MyGameManager : MonoBehaviour  {
	
	public GameObject sparty;
	public GameObject tank;
	public CameraControl m_CameraControl;

	void Start() {
		SetCameraTargets();
	}

	void Update() {

	}

	private void SetCameraTargets() {
        
		// Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[2];

        targets[0] = tank.transform;
		targets[1] = sparty.transform;

        // These are the targets the camera should follow.
    	m_CameraControl.m_Targets = targets;
    }
}