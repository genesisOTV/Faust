using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questManager : MonoBehaviour {
	[SerializeField] private List<GameObject> questNPCs;
	[SerializeField] private GameObject Player;
	private playerStatusLog pStatus;
	private IqBase M_qLoader;
	private List<IqBase> qStore;
	private List<questAI> NPC_AIs = new List<questAI> ();
	//Level Specifier (int)
	public int levelLoad;

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void Awake(){
		//Assignment of pStatus
		Messenger<bool, int>.AddListener("MnPackAssignment", runQPack_Mn);
		pStatus = Player.GetComponent<playerStatusLog> ();
		//Create/Fill List of Quests
 		StartCoroutine (compileQList ());
		//Get Quest-wide NPCs
		foreach (GameObject npc in questNPCs) {
			NPC_AIs.Add (npc.GetComponent<questAI> ());
		}
		//Distribute desired quest's attribute
		StartCoroutine (distributeQ ());

		//**TEMPORARY TESTER**
		//runQPack_Mn (true, 1);
	}
	private IEnumerator compileQList(){
		qStore = new List<IqBase> ();
		//Compile all quest packages:
		questPackageI qI = gameObject.AddComponent<questPackageI> ();
		qStore.Add (qI);
		yield return null;
	}

	private IEnumerator distributeQ(){
		M_qLoader = qStore [levelLoad] as IqBase;
		foreach (questAI ai in NPC_AIs) {
			ai.AI_qLoader = qStore [levelLoad];
		}
		yield return null;
	}

	//Runs managerial qPack assigned via Messenger Broadcast
	public void runQPack_Mn(bool runQPack, int qPackLoad){
		Debug.Log ("MnQuest Called");
		if (runQPack) {
			M_qLoader.qPack_Mn (qPackLoad);
		} else {
			//Assign to inactive state:
			qPackLoad = -1;
		}
	}

	void Update () {
		//Run Managerial qPack:
		//M_qLoader.qPack_Mn(pStatus);
		
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
