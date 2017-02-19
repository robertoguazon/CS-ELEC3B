using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputActionType {
	ZOOM_OUT_CAMERA = 0,
	ZOOM_IN_CAMERA = 1,
	ROTATE_Y_AXIS = 2,
	ROTATE_X_AXIS = 3,
	GRAB_PIECE = 4,
	PLACE_PIECE = 5,
	EAT_PIECE = 6,
}

public class InputManager : MonoBehaviour {
	
	public delegate void InputEventHandler(InputActionType actionType);
	public static event InputEventHandler InputEvent;

	private bool clicked;

	void Update() {
		if (InputEvent == null) return;

		if (!GameManager.Instance.IsReady) return;
		if (Input.GetMouseButtonUp(0)) {
			Debug.Log("INPUT EVENT");
			InputEvent(InputActionType.GRAB_PIECE);
		} 
	}


}
