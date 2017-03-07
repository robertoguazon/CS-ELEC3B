using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : InputReceiver {

	public float moveSpeed = 3f;
	public float rotateSpeed = 3f;
	public float jumpStrength = 5f;

	private bool walking = false;
	private Rigidbody rb;
	private Animator anim;

	private Vector3 moveVector;
	private Transform mainCamTrans;

	private int points = 0;
	private int coints = 0;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();

		moveVector = Vector3.zero;
		mainCamTrans = GameManager.Instance.mainCamera.transform;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		if (moveVector.magnitude > 1) moveVector.Normalize();

		moveVector = RotateWithView();
		rb.AddForce(moveVector * moveSpeed);
		moveVector = Vector3.zero;
	}

	protected override void OnInputEvent(InputActionType iat) {
		/*
		switch (iat) {
			case InputActionType.MOVE_DOWN:
				rb.MovePosition(transform.position + transform.forward * -moveSpeed * Time.deltaTime);
				walking = true;
				break;
			case InputActionType.MOVE_LEFT:
				rb.MovePosition(transform.position + transform.right * -moveSpeed * Time.deltaTime);
				break;
			case InputActionType.MOVE_RIGHT:
				rb.MovePosition(transform.position + transform.right * moveSpeed * Time.deltaTime);
				break;
			case InputActionType.MOVE_UP:
				rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
				walking = true;
				break;
			case InputActionType.ROTATE_Y_RIGHT:
				transform.Rotate(Vector3.up * rotateSpeed);
				break;
			case InputActionType.ROTATE_Y_LEFT:
				transform.Rotate(Vector3.up * -rotateSpeed);
				break;
			case InputActionType.IDLE:
				walking = false;
				break;
			case InputActionType.JUMP:
				Jump();
				break;
		}
		*/

		switch (iat) {
			case InputActionType.MOVE_DOWN:
				if (moveVector.z >= 0) moveVector.z--;
				walking = true;
				break;
			case InputActionType.MOVE_LEFT:
				if (moveVector.x >= 0) moveVector.x--;
				break;
			case InputActionType.MOVE_RIGHT:
				if (moveVector.x <= 0) moveVector.x++;
				break;
			case InputActionType.MOVE_UP:
				if (moveVector.z <= 0) moveVector.z++;
				walking = true;
				break;
			case InputActionType.ROTATE_Y_RIGHT:
				break;
			case InputActionType.ROTATE_Y_LEFT:
				break;
			case InputActionType.IDLE:
				walking = false;
				break;
			case InputActionType.JUMP:
				Jump();
				break;
		}

		anim.SetBool("Walking",walking);
	}

	Vector3 RotateWithView() {
		if (mainCamTrans == null) {
			mainCamTrans = GameManager.Instance.mainCamera.transform;
		}
		if (mainCamTrans == null) return moveVector;

		Vector3 dir = mainCamTrans.TransformDirection(moveVector);
		dir.Set(dir.x, 0, dir.z);
		return dir.normalized * moveVector.magnitude;

	}

	void Jump() {
		rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
		walking = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "coin") {
			Coin coinScript = other.GetComponent<Coin>();
			points += coinScript.points;
			coints++;
			Destroy(other.gameObject);

			Debug.Log("Points: " + points);
		}
	}
	
}
