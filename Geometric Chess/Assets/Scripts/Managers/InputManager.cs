using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputActionType {
	ZOOM_CAMERA = 0,
	ROTATE_Y_AXIS = 1,
	ROTATE_X_AXIS = 2,
	GRAB_PIECE = 3,
	PLACE_PIECE = 4,
	EAT_PIECE = 5,
	CANCEL_PIECE = 6,
}

public class InputManager : Singleton<InputManager> {
	
	public delegate void InputEventHandler(InputActionType actionType);
	public static event InputEventHandler InputEvent;

	private bool clicked;

	void Awake() {
		_destroyOnLoad = destroyOnLoad;
	}

	void Update() {
		if (InputEvent == null) return;

		if (!GameManager.Instance.IsReady) return;
		if (Input.GetMouseButtonUp(0)) {
			if (GameManager.Instance.GameState.IsWaiting) {
				InputEvent(InputActionType.GRAB_PIECE);
			}
		}

		if (Input.GetMouseButtonUp(1)) {
			if (GameManager.Instance.GameState.IsHolding) {
				InputEvent(InputActionType.CANCEL_PIECE);
			}
		}
	}


}
