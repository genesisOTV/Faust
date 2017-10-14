/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcAI : MonoBehaviour {
	public bool canInteract;
	public bool canDie;

	private npcManager selfConfig;
	private npcHealth selfHealth;
	private bool doingQuest;
	private int HPinst;
	public int dmgIntake;

	void Start(){
		//[PROGRAMMER] nQ's Script [PROGRAMMER]
		selfConfig = this.GetComponent<npcManager> ();
		selfHealth = selfConfig.Health;
		HPinst = selfHealth.initialHP;
	}

	void OnTriggerStay2D(Collider2D player){
		//[PROGRAMMER] nQ's Script [PROGRAMMER]
		//Collider Verification
		if (player.CompareTag ("User")) {
			int questLevel = player.GetComponent<playerStatusLog> ().questStage;
			//Actions for recognized interactions
			if (Input.GetKeyDown (KeyCode.E) && canInteract) {
				Debug.Log ("Successfully interacted");
				doingQuest = player.GetComponent<playerStatusLog> ().onQuest;
				StartCoroutine (loadQuest (questLevel));
			} else if (Input.GetKeyDown (KeyCode.F) && canDie) {
				Debug.Log ("Punched");
				HPinst = HPinst - dmgIntake;
				selfHealth.currentHP = HPinst;
			}
		}
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER]
	private IEnumerator loadQuest(int questLevel){
		if (questLevel == 0 && !doingQuest) {
			Debug.Log ("Loading Section #1 of Quest #1");
			Debug.Log ("Begin Section #1, Quest #1");
			doingQuest = true;
			Messenger<bool>.Broadcast (messengerStrings.questStarted, true);

		} else if(questLevel == 1) {
			Debug.Log ("Completed Quest");
		}

		yield return null;
	}
}
*/
