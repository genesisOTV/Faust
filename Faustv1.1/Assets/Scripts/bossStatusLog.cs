using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossStatusLog : MonoBehaviour {
	public bool isIgnitable;
	public bool hitByBarrel;
	public bool onFire;

	//Boss damage handler of torch attack
	public float torchAtkHandler(float defaultDmg){
		//Determines type of damage
		if (isIgnitable) {
			Debug.Log ("Boss set on fire");
			onFire = true;
			float modedDmg = defaultDmg * 2;
			return modedDmg;
		} else {
			Debug.Log ("Torch damage dealt");
			return defaultDmg;
		}
	}
}
