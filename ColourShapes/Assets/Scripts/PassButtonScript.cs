using UnityEngine;
using System.Collections;

public class PassButtonScript : MonoBehaviour {

	public void OnClick(){
		GameManagerScript.Instance.changePlayer ();
	}
}
