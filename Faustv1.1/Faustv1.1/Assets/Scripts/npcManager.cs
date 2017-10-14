using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcManager : MonoBehaviour {
	public bool doesDestruct;

	public npcHealth Health;
	public int defaultHP;
	private bool Alive;

	//[PROGRAMMER] nQ's Script [PROGRAMMER]
	void Awake(){
		Health = this.GetComponent<npcHealth> ();
		Health.initialHP = defaultHP;
		Health.Alive = true;
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER]
	void Update () {
		Alive = Health.Alive;
		if (!Alive && doesDestruct) {
			Messenger.Broadcast (messengerStrings.enemyKilled);
			Debug.Log ("Killed");
			Destroy (this.gameObject, (float)0.1);
		}
	}
}
