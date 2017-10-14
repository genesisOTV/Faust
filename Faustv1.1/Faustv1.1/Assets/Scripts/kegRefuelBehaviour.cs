using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kegRefuelBehaviour : MonoBehaviour {
	public Vector2 interactOffset;
	private PlayerController movementScript;
	private GameObject Player;
	private torchBehaviour torchRef;
	private Vector2 Pos;
	private Vector2 interactPos;
	private bool withinCollider = false;
	private bool isSelected = false;
	//Assigning important variables
	void Awake(){
		Pos = transform.position;
		interactPos = Pos + interactOffset;
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Interactor){
		if (Interactor.gameObject.tag == "User") {
			Player = Interactor.gameObject;
			withinCollider = true;
			movementScript = Player.GetComponent<PlayerController> ();

			if (movementScript.isCarryingTorch) {
				torchRef = movementScript.Torch;
				if (isSelected) {
					//Refuel behaviour
					if (Input.GetKeyDown (KeyCode.E)) {
						//Sets player position
						movementScript.movePlayer (interactPos);
						if (movementScript.completedMove) {
							Messenger<bool>.Broadcast ("canMove_Update", false);
						}
						//Refuels torch
						torchRef.fullyFueled = true;
						//ADD ANIMATION
						Messenger<bool>.Broadcast ("canMove_Update", true);
						isSelected = false;
					}
				}
			}
		}
	}
	//Detects selection of gameObject (mouse click)
	void OnMouseOver(){
		if (withinCollider) {
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("Selected");
				isSelected = true;
			}
		}
	}

	//Deselects gameObject upon collider exit
	void OnTriggerExit2D(Collider2D Interactor){
		if (Interactor.gameObject.tag == "User") {
			withinCollider = false;
			if (isSelected) {
				isSelected = false;
			}
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
