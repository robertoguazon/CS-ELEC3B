using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : InputReceiver {

	public GameObject target;
	public float rotateAngleSpeed = 5f;

	private Vector3 offset;
	private float offsetRotY;

	// Use this for initialization
	void Start () {
		offset = transform.position - target.transform.position;
		offsetRotY = transform.eulerAngles.y - target.transform.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void LateUpdate() {
		transform.position = target.transform.position + offset;
		transform.position = MathUtil.RotatePointAroundPivot(transform.position, target.transform.position, Vector3.up * (target.transform.eulerAngles.y + offsetRotY));
		transform.LookAt(target.transform.position);
	}
	
}
