using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorBehaviour : MonoBehaviour {
	public Vector2 Destination;
	public int doorType;
	public bool entranceStatus;
	//Assignment of important variables
	void Awake(){
		entranceStatus = false;
		Messenger<bool, int>.AddListener ("(un)lockCellar", lockUpdate);
	}
	//Allows door accessibility to be controlled via Messenger Broadcast
	public void lockUpdate(bool statusUpdate, int updatedDoor){
		if (doorType == updatedDoor) {
			entranceStatus = statusUpdate;
		}
	}


	void OnTriggerStay2D(Collider2D interactingColl){
		Debug.Log ("Within door trigger");
		if (entranceStatus) {
			//Ensures interactor is player
			GameObject interactingObj = interactingColl.gameObject;
			if (interactingObj.tag == "User") {
				interactingObj.transform.position = Destination;
				//Broadcasts information depending on door type (Entry/Exit)
				if (doorType == 1) {
					Debug.Log ("Entered Cellar -doorBehaviour");
					Messenger<bool>.Broadcast ("inCellarUpdate", true);
				} else if (doorType == 2) {
					Debug.Log ("Left Cellar -doorBehaviour");
					Messenger<bool>.Broadcast ("inCellarUpdate", false);
				}
			}
		} else {
			Debug.Log ("Complete quest to use door");
		}
	}
}
