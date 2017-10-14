using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorBehaviour : MonoBehaviour {
	public Vector2 Destination;
	public int doorType;
	public bool entranceStatus;

	void Awake(){
		entranceStatus = false;
		Messenger<bool, int>.AddListener ("(un)lockCellar", lockUpdate);
	}
	public void lockUpdate(bool statusUpdate, int updatedDoor){
		if (doorType == updatedDoor) {
			entranceStatus = statusUpdate;
		}
	}

	void OnTriggerStay2D(Collider2D interactingColl){
		Debug.Log ("Within door trigger");
		if (entranceStatus) {
			GameObject interactingObj = interactingColl.gameObject;
			if (interactingObj.tag == "User") {
				interactingObj.transform.position = Destination;
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
