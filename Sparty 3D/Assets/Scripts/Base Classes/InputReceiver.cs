using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiver : MonoBehaviour {

	protected virtual void OnEnable() {
		InputManager.InputEvent += OnInputEvent;
	}

	protected virtual void OnDisable() {
		InputManager.InputEvent -= OnInputEvent;
	}

	protected virtual void OnInputEvent(InputActionType inputActionType) {

	}
}
