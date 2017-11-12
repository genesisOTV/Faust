using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kegRefuelBehaviour : MonoBehaviour {
	private PlayerController movementScript;
	private GameObject Player;
	private torchBehaviour torchRef;
	private bool withinCollider = false;

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Interactor){
		if (Interactor.gameObject.tag == "User") {
			Player = Interactor.gameObject;
			withinCollider = true;
			movementScript = Player.GetComponent<PlayerController> ();

			if (movementScript.isCarryingTorch) {
				torchRef = movementScript.Torch;
				//if (isSelected) {
					//Refuel behaviour
					if (Input.GetKeyDown (KeyCode.E)) {
						//Refuels torch
						//ADD ANIMATION
						torchRef.fullyFueled = true;
					}
				//}
			}
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
