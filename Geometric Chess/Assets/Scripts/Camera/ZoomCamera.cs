using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour, IInputReceiver {

	[SerializeField]
	private int sensitivity = 5;
	[SerializeField]
	private int minFOV = 20;
	[SerializeField]
	private int maxFOV = 80;

	private Camera zoomCamera;
	private float fieldOfView;

	// Use this for initialization
	void Start() {
		zoomCamera = GetComponent<Camera>();
		fieldOfView = zoomCamera.fieldOfView;
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
			case InputActionType.ZOOM_IN:
				fieldOfView -= sensitivity;
				break;
			case InputActionType.ZOOM_OUT:
				fieldOfView += sensitivity;
				break;
		}

		fieldOfView = Mathf.Clamp(fieldOfView,minFOV,maxFOV);
		zoomCamera.fieldOfView = fieldOfView;
	}
}
