using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectiveBehaviour : MonoBehaviour {
	private carriableBehaviour carriedProp;
	private bool isInteractable = true;
	private Collider2D solidBody;
	//Assignment of important variables
	void Awake(){
		Collider2D[] Colliders = GetComponents<Collider2D> ();
		carriedProp = GetComponent<carriableBehaviour> ();
		//Sets solidBody to appropriate collider
		foreach (Collider2D collider in Colliders) {
			if (!collider.isTrigger) {
				solidBody = collider;
			}
		}
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Interactor){
		if (Interactor.gameObject.tag == "User") {
			GameObject Player = Interactor.gameObject;
			//Behaviour on key press
			if (Input.GetKeyDown (KeyCode.E) && isInteractable) {
				Debug.Log ("Interacted with objective");
				//Setting proxy PlayerController variables
				PlayerController Movement = Player.GetComponent<PlayerController> ();
				Movement.isCarryingObj = true;
				Movement.carriedObject = this.gameObject;
				DontDestroyOnLoad (transform.gameObject);
				transform.SetParent (Player.transform);
				transform.localPosition = Movement.Orientation * carriedProp.carriedOffset;
				//Disabling solid collider to prevent eratic movement
				solidBody.enabled = false;
				//Setting render layer to Player's
				SpriteRenderer Renderer = GetComponent<SpriteRenderer> ();
				Renderer.sortingOrder = 1;

				//Relates information to questPackages via Messenger Broadcast
				Messenger.Broadcast ("objectiveIReached");
				isInteractable = false;
			}
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
