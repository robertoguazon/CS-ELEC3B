using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : InputReceiver {

	public GameObject target;
	public float rotateAngleSpeed = 5f;

	private Vector3 offset;
	private float offsetRotY;
	private float addRotY;
	private Transform targetTransform;

	// Use this for initialization
	void Start () {
		targetTransform = target.transform;

		offset = transform.position - targetTransform.position;
		offsetRotY = transform.eulerAngles.y - 0;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void LateUpdate() {
		transform.position = targetTransform.position + offset;
		transform.position = MathUtil.RotatePointAroundPivot(transform.position, targetTransform.position, Vector3.up * (addRotY + offsetRotY));
		transform.LookAt(targetTransform.position);
	}

	protected override void OnInputEvent(InputActionType iat) {
		switch (iat) {
			case InputActionType.ROTATE_Y_RIGHT:
				addRotY += rotateAngleSpeed * Time.deltaTime;
				break;
			case InputActionType.ROTATE_Y_LEFT:
				addRotY += -rotateAngleSpeed * Time.deltaTime;
				break;
		}
	}

	protected override void OnEnable() {
		InputManager.LateInputEvent += OnInputEvent;
	}

	protected override void OnDisable() {
		InputManager.LateInputEvent -= OnInputEvent;
	}
	
}
