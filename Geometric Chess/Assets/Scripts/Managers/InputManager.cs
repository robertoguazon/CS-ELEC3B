using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputActionType {
	GRAB_PIECE = 0,
	PLACE_PIECE = 1,
	CANCEL_PIECE = 2,
	ZOOM_IN = 3,
	ZOOM_OUT = 4,
	ROTATE_UP = 5,
	ROTATE_DOWN = 6,
	ROTATE_LEFT = 7,
	ROTATE_RIGHT = 8,
}

public class InputManager : Singleton<InputManager> {
	
	public delegate void InputEventHandler(InputActionType actionType);
	public static event InputEventHandler InputEvent;

	private bool clicked;
	private Node currentNode;
	private GCPlayer currentPlayer;

	void Awake() {
		_destroyOnLoad = destroyOnLoad;
	}

	void Update() {
		if (InputEvent == null) return;

		if (!GameManager.Instance.IsReady) return;

		HighlightTile();

		if (Input.GetMouseButtonUp(0)) {
			if (GameManager.Instance.GameState.IsWaiting) {
				InputEvent(InputActionType.GRAB_PIECE);
			} else if (GameManager.Instance.GameState.IsHolding) {
				InputEvent(InputActionType.PLACE_PIECE);
			}
		}

		if (Input.GetMouseButtonUp(1)) {
			if (GameManager.Instance.GameState.IsHolding) {
				InputEvent(InputActionType.CANCEL_PIECE);
			}
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			InputEvent(InputActionType.ZOOM_IN);
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			InputEvent(InputActionType.ZOOM_OUT);
		}

		if (Input.GetMouseButton(2)) {
			float mouseX = Input.GetAxis("Mouse X");
			if (mouseX > 0) {
				InputEvent(InputActionType.ROTATE_RIGHT);
			} else if (mouseX < 0) {
				InputEvent(InputActionType.ROTATE_LEFT);
			}

			float mouseY = Input.GetAxis("Mouse Y");
			if (mouseY > 0) {
				InputEvent(InputActionType.ROTATE_UP);
			} else if (mouseY < 0) {
				InputEvent(InputActionType.ROTATE_DOWN);
			}
		}
	}

	void HighlightTile() {
		if (GameManager.Instance.GameState.IsWaiting) {
			if (currentNode != null) {
				currentNode.SetMaterialOriginal();
			}
			currentNode = Finder.RayHitFromScreen<Node>(Input.mousePosition);
			if (currentNode != null) {
				Piece piece = currentNode.Piece;
				if (piece != null) {
					if (GameManager.Instance.CurrentPlayer.Has(piece)) {
						currentNode.MoveHighlight();
					} else {
						currentNode.EatHighlight();
					}
				}
			}
		}
	}

}
