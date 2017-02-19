using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Scalable {

	[SerializeField]
	protected float speed = 2f;

	protected IEnumerator moveIEnumerator;

	protected override void Start() {
		base.Start();
	}

	public void MoveBy(Vector3 addPos) {
		StopMoveIEnumerator();
		moveIEnumerator = IEMoveBy(addPos);
		StartCoroutine(moveIEnumerator);
	}

	protected void StopMoveIEnumerator() {
		if (moveIEnumerator != null) {
			StopCoroutine(moveIEnumerator);
		}
	}

	protected virtual IEnumerator IEMoveBy(Vector3 addPos) {
		ready = false;
		Vector3 origPos = transform.position;
		Vector3 targPos = origPos + addPos;
		
		while (true) {
			if (IsNear(targPos)) {
				ready = true;
				yield break;
			}
			transform.position = Vector3.MoveTowards(transform.position,targPos,speed * Time.deltaTime);

			yield return null;
		}
	}

	protected virtual bool IsNear(Vector3 targPos) {
		return (transform.position - targPos).sqrMagnitude < .01f;
	}
}
