/*
*Chip class
*contains all information about the chip;
*/

using UnityEngine;
using System.Collections;

public class ChipScript : MonoBehaviour {

	public Sprite red;
	public Sprite blue;
	public string chipColor = "none";

	private SpriteRenderer rend;

	void Start () {
		rend = GetComponent<SpriteRenderer> ();
	}

	/*
		Shows the chip with corresponding color
	*/
	public void ShowChip(string color){
		chipColor = color;
		if (chipColor == "red") {
			rend.sprite = red;
		}
		else if (chipColor == "blue"){
			rend.sprite = blue;
		}
		rend.enabled = true;

	}

}
