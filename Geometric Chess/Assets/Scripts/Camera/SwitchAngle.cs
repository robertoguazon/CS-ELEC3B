using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAngle : MonoBehaviour {

	[SerializeField]
	private Vector3 p1CameraPos = new Vector3(-0.75f, 5.82f, -6.01f);
	[SerializeField]
	private Vector3 p1CameraRot = new Vector3(49.04f, 4.72f, 0);

	[SerializeField]
	private Vector3 p2CameraPos = new Vector3(0.75f, 5.82f, 6.01f);
	[SerializeField]
	private Vector3 p2CameraRot = new Vector3(49.04f, 184.72f, 0);

	private Camera rotateCamera;

	// Use this for initialization
	void Start () {
			rotateCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchCamera(PlayerType playerType) {
		switch (playerType) {
			case PlayerType.P1:
				SetCameraPosRot(p1CameraPos, p1CameraRot);
				break;
			case PlayerType.P2:
				SetCameraPosRot(p2CameraPos, p2CameraRot);
				break;
		}
	}

	private void SetCameraPosRot(Vector3 pos, Vector3 rot) {
		rotateCamera.transform.position = pos;
		rotateCamera.transform.eulerAngles = rot;
	}
}
