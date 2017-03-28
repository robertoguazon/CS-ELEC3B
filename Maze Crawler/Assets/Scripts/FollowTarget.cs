using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

	private GameObject target;
	private Rigidbody rb;
	private float velocity;

	public GameObject Target {
		set {
			target = value;
		}

		get {
			return target;
		}
	}

	public float Velocity {
		get {return velocity;}
		set {velocity = value;}
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) return;
		
		transform.LookAt(target.transform);
		rb.velocity = velocity * transform.forward;
	}
}
