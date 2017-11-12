using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteHolder : MonoBehaviour {
	[SerializeField] private Sprite[] Halves;
	public Sprite lSprite;
	public Sprite rSprite;

	public void setPoles(int Quadrant){
		int baseIndex = -1;
		switch (Quadrant) {
		case 1:
			Debug.Log ("Barrel oriented facing upwards");
			baseIndex = 0;
			break;
		case 2:
			Debug.Log ("Barrel oriented facing rightwards");
			baseIndex = 2;
			break;
		case 3:
			Debug.Log ("Barrel oriented facing downwards");
			baseIndex = 4;
			break;
		case 4:
			Debug.Log ("Barrel oriented facing leftwards");
			baseIndex = 6;
			break;
		}
		lSprite = Halves [baseIndex];
		rSprite = Halves [baseIndex + 1];
	}
}
