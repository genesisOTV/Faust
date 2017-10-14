using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcHealth : MonoBehaviour {
	public int initialHP;
	public int currentHP;
	public bool Alive;

	//[PROGRAMMER] nQ's Script [PROGRAMMER]
	void Start(){
		currentHP = initialHP;
		Debug.Log (currentHP);
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER]
	void Update () {
		if (currentHP <= 0 && Alive) {
			Debug.Log ("NPC Killed");
			Alive = false;
		}
	}
}
