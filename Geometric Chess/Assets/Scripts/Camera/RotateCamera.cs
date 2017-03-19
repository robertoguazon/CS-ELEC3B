using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour, IInputReceiver {

	[SerializeField]
	private Transform target;
	[SerializeField]
	private float horizAngleMove;
	[SerializeField]
	private float vertAngleMove;

	[SerializeField]
	private float minVertAngle;
	[SerializeField]
	private float maxVertAngle;

	[SerializeField]
	private Vector3 p1CameraPos = new Vector3(-0.75f, 5.82f, -6.01f);
	[SerializeField]
	private Vector3 p1CameraRot = new Vector3(49.04f, 4.72f, 0);

	[SerializeField]
	private Vector3 p2CameraPos = new Vector3(0.75f, 5.82f, 6.01f);
	[SerializeField]
	private Vector3 p2CameraRot = new Vector3(49.04f, 184.72f, 0);

	private Camera rotateCamera;

	void Start() {
		rotateCamera = GetComponent<Camera>();
		EnableInput();
	}

	public void EnableInput() {
		InputManager.InputEvent += OnInputEvent;
	}

	public void DisableInput() {
		InputManager.InputEvent -= OnInputEvent;
	}

	public void OnInputEvent(InputActionType action) {
		switch (action) {
			case InputActionType.ROTATE_UP:
				//RotateVertical(true);
				break;
			case InputActionType.ROTATE_DOWN:
				//RotateVertical(false);
				break;
			case InputActionType.ROTATE_LEFT:
				RotateHorizontal(true, horizAngleMove);
				break;
			case InputActionType.ROTATE_RIGHT:
				RotateHorizontal(false, horizAngleMove);
				break;
		}
	}

	private void RotateHorizontal(bool left, float yAngle) {
		float dir = 1;
		if (! left) dir = -1;
		
		transform.RotateAround(target.position, Vector3.up, (yAngle * dir));
	}

	private void RotateVertical(bool up, float xAngle) {
		float dir = 1;
		if (! up) dir = -1;

		transform.RotateAround(target.position, transform.TransformDirection(Vector3.right), xAngle * dir);
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
