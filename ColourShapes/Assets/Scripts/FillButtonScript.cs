using UnityEngine;
using System.Collections;

public class FillButtonScript : MonoBehaviour {

	public void OnClick(){
		GameManagerScript.Instance.fillEmptyCells ();
	}
}
