using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleItem : MonoBehaviour {

	public float rotateYSpeed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * rotateYSpeed * Time.deltaTime);
	}
}
