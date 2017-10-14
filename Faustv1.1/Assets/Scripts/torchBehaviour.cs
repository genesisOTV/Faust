using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class torchBehaviour : MonoBehaviour {
	//Public vars
	public bool doesExtinguish = false;
	public bool toggleTorchLighting;
	public float Displacement_fwd;
	public float Displacement_dwn;
	public float ignitionLimit;
	public float timeRemaining;
	public bool fullyFueled;
	//Private vars
	private carriableBehaviour carriableProp;
	private PlayerController directionalRef;
	private Slider flameTimeBar;
	private bool canExtinguish;
	private GameObject Player;
	private Light cellarLight;
	private bool trackPlayer;
	private Image Darkout;
	private bool isHeld;
	//Assignment of important variables
	void Awake(){
		if (toggleTorchLighting) {
			flameTimeBar = GameObject.FindGameObjectWithTag ("ftTimer").GetComponentInChildren<Slider> ();
			flameTimeBar.gameObject.SetActive (false);
			doesExtinguish = true;
		}
		carriableProp = GetComponent<carriableBehaviour> ();
		fullyFueled = true;
		isHeld = false;
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Interactor){
		if (Interactor.gameObject.tag == "User") {
			Player = Interactor.gameObject;
			directionalRef = Player.GetComponent<PlayerController> ();
			if (cellarLight == null) {
				cellarLight = Player.GetComponentInChildren<LightsScript> ().GetComponentInChildren<Light> ();
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				//Executed when picked up (only ran once)
				if (!isHeld) {
					isHeld = true;
					Debug.Log ("Grabbed torch");
					if (toggleTorchLighting) {
						flameTimeBar.gameObject.SetActive (true);
					}
					//Setting parent for synced movement
					transform.SetParent (Player.transform);
					transform.localPosition = directionalRef.Orientation * carriableProp.carriedOffset;

					//Assigning necessary player properties
					GetComponent<SpriteRenderer> ().sortingOrder = 1;
					directionalRef.isCarryingTorch = true;
					directionalRef.Torch = this;
				}
				//Executed when torch is fully fueled
				if (fullyFueled && toggleTorchLighting) {
					updateTorchDisplay (true, true);
					fullyFueled = false;
				}
			}
		}
	}
	//Controls blackout screen type & timer
	public void updateTorchDisplay(bool isHandled, bool isOn){
		if (isOn) {
			cellarLight.cookieSize = 4;
			canExtinguish = true;
			timeRemaining = ignitionLimit;
		} else {
			cellarLight.cookieSize = 1.8f;
			canExtinguish = false;
			timeRemaining = 0;
		}

		if (!flameTimeBar.gameObject.activeSelf) {
			flameTimeBar.gameObject.SetActive (true);
		}
	}


	void Update(){
		//Torch flame timer
		if (doesExtinguish) {
			if (canExtinguish) {
				timeRemaining -= Time.deltaTime;
				flameTimeBar.value = timeRemaining / ignitionLimit;
				if (timeRemaining <= 0) {
					Debug.Log ("Torch light extinguished");
					updateTorchDisplay (true, false);
				}
			}
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
