using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil {

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		return Quaternion.Euler(angles) * (point - pivot) + pivot;
	}
}
