using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputActionType {
	MOVE_LEFT = 0,
	MOVE_RIGHT = 1,
	MOVE_UP = 2,
	MOVE_DOWN = 3,
	ROTATE_Y_RIGHT = 4,
	ROTATE_Y_LEFT = 5,
	IDLE = 6,
	JUMP = 7,
	NONE = -1,
}

[System.Serializable]
public struct InputActionKeyMap {

	public InputActionType ActionTypeHold;
	public InputActionType ActionTypeReleased;
	public KeyCode Key;
}

public class InputManager : Singleton<InputManager> {

	public delegate void InputEventHandler(InputActionType inputActionType);
	public static event InputEventHandler InputEvent;
	public static event InputEventHandler LateInputEvent;

	[Header("KeyMap - Hold")]
	public List<InputActionKeyMap> KeyMapHold = new List<InputActionKeyMap>();
	private int numberOfHoldKeys;

	// Use this for initialization
	void Start () {
		numberOfHoldKeys = KeyMapHold.Count;
	}
	
	// Update is called once per frame
	void Update () {
		if (InputEvent == null) return;

		for (byte i = 0; i < numberOfHoldKeys; i++) {
            if (Input.GetKey(KeyMapHold[i].Key)) {
                InputEvent(KeyMapHold[i].ActionTypeHold);
                //return;
            }
        }

		for (byte i = 0; i < numberOfHoldKeys; i++) {
            if (Input.GetKeyUp(KeyMapHold[i].Key)) {
                InputEvent(KeyMapHold[i].ActionTypeReleased);
                //return;
            }
        }

		float mouseX = Input.GetAxis("Mouse X");
		if (mouseX > 0) {
			InputEvent(InputActionType.ROTATE_Y_RIGHT);
		} else if (mouseX < 0) {
			InputEvent(InputActionType.ROTATE_Y_LEFT);
		}

	}

	void LateUpdate() {
		if (LateInputEvent == null) return;
		for (byte i = 0; i < numberOfHoldKeys; i++) {
            if (Input.GetKey(KeyMapHold[i].Key)) {
                LateInputEvent(KeyMapHold[i].ActionTypeHold);
                //return;
            }
        }

		float mouseX = Input.GetAxis("Mouse X");
		if (mouseX > 0) {
			LateInputEvent(InputActionType.ROTATE_Y_RIGHT);
		} else if (mouseX < 0) {
			LateInputEvent(InputActionType.ROTATE_Y_LEFT);
		}
		
	}
}
