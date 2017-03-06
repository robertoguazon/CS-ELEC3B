using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : InputReceiver {

	public float moveSpeed = 3f;
	public float rotateSpeed = 3f;

	private bool walking = false;
	private Rigidbody rb;
	private Animator anim;

	private int points = 0;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	protected override void OnInputEvent(InputActionType iat) {
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
		}

		anim.SetBool("Walking",walking);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "coin") {
			Coin coinScript = other.GetComponent<Coin>();
			points += coinScript.points;
			Destroy(other.gameObject);

			Debug.Log("Points: " + points);
		}
	}
	
}
