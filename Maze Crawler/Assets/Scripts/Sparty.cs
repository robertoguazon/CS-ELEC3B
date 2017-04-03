using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

public class Sparty : MonoBehaviour {

	public Canvas canvas;
	public GameObject target;
	public float speed = 2f;
	public float damage = 5f;
	public float cdSeconds = 4f;
	public float targetRadius = 10f;

	private float timeStamp;

	// Use this for initialization
	void Awake () {
		DeactivateHealthBar();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, target.transform.position) <= targetRadius) 
			FollowTarget();
	}

	void FollowTarget() {
		Vector3 targetPostition = new Vector3( 
			 							target.transform.position.x, 
                                        transform.position.y, 
                                        target.transform.position.z ) ;
		transform.LookAt(targetPostition);
		transform.Translate(Vector3.forward * Time.deltaTime * speed, transform);
	}

	public void ActivateHealthBar() {
		canvas.enabled = true;
	}

	public void DeactivateHealthBar() {
		canvas.enabled = false;
	}

	void OnDestroy() {
		if (MyGameManager.Instance != null) {
			MyGameManager.Instance.RemoveSparty(this);
		}
		Debug.Log("Dying");
	}

	void OnCollisionStay(Collision collision) {
		Collider other = collision.collider;
		if (other.tag == "Tank") {
			if (timeStamp <= Time.time) {
				other.GetComponent<TankHealth>().TakeDamage(damage);
				timeStamp = Time.time + cdSeconds;
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position,targetRadius);
	}
}
