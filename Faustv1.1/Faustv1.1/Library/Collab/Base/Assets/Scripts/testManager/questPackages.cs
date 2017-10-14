using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class questPackages : MonoBehaviour{
	//!!Keep class for name reference!!
}
//Interface of all quest-loads
public interface IqBase{
	//Declaration of inherit quest methods
	void qPack_AI(string callTag);
	void qPack_Mn(playerStatusLog pStatus);
}

//QUESTS GO HERE; INCLUDE INTERFACE "IqBase" FOR SEAMLESS INTEGRATION:
public class questPackageI : MonoBehaviour, IqBase{
	//Fill manually for quest run path
	private limbo_chantDictionaries textLog;
	private DialogueManager dlgInterface;
	private Interactable dDictionaryArchive;
	private GameObject dlgManager;
	private GameObject Prompter;
	private GameObject BLPrompter;
	private GameObject Player;
	private GameObject btnLoad_Siebel;
	private GameObject btnLoad_Frosch;
	private GameObject lEAccessDB;
	private GameObject tDialogueCanvas;

	//REFORMAT
	private bool inputReceived;
	private bool canContinue;
	private bool success_stgI;
	private bool success_stgII;
	private bool DBgate;
	private bool FWgate;
	private bool TCgate;
	private int inputReturn;
	private List<int> stgRef;
	private int activeQNPC;
	private playerStatusLog qStatus;
	private Canvas dBCanvas;
	private int DBTally;
	private Text tempTxt;
	private Text modelDB;
	private bool songSuccess;
	private string cont1Getter;
	private string cont2Getter;

	void Awake(){
		stgRef_Setup ();

		tDialogueCanvas = GameObject.FindGameObjectWithTag ("testerCanvas");
		dDictionaryArchive = tDialogueCanvas.GetComponent<Interactable> ();
		dlgManager = GameObject.FindGameObjectWithTag ("DialogueManager");
		dlgInterface = dlgManager.GetComponent<DialogueManager> ();

		Player = GameObject.FindGameObjectWithTag ("User");
		Prompter = GameObject.FindGameObjectWithTag ("tBox");
		BLPrompter = GameObject.FindGameObjectWithTag ("blBox");
		btnLoad_Siebel = Resources.Load ("Prefabs/buttonLoads/Siebel_1q1") as GameObject;
		btnLoad_Frosch = Resources.Load ("Prefabs/buttonLoads/Frosch_1q2") as GameObject;
		textLog = gameObject.AddComponent<limbo_chantDictionaries> ();

		qStatus = Player.GetComponent<playerStatusLog> ();
		dBCanvas = Prompter.GetComponentInChildren<Canvas> ();
		modelDB = dBCanvas.GetComponentInChildren<Text> ();
		DBTally = 0;
		canContinue = false;
		lEAccessDB = new GameObject ();
		//Re-locate
		Prompter.SetActive (false);

     
	}
	public void stgRef_Setup(){
		stgRef = new List<int> ();
		//Add foreach with # of NPCs
		stgRef.Add (0);
		stgRef.Add (0);
		stgRef.Add (0);
	}

	public void compileDecision(int btnSelection){
		inputReceived = true;
		inputReturn = btnSelection;
	}
	//CREATE HANDLER FOR 2 STRINGS
	public void limbo_compileDecision(string Cont1, string Cont2, int Choice){
		cont1Getter = Cont1;
		if (Cont2 != "Holder") {
			cont2Getter = Cont2;
		} else {
			cont2Getter = "Empty";
		}
		inputReceived = true;
		inputReturn = Choice;
	}
	public void compileResponseII_i(bool completionType){
		songSuccess = completionType;
		inputReceived = true;
	}
	public void updateLEResponse(string lEInput){
		Text lEDBTxt = lEAccessDB.GetComponent<Text> ();
		lEDBTxt.text = "Faust: " + lEInput;
	}

	public void qPack_AI(string caller){
		if (qStatus.onQuest == 0) {
			//SET TO ZERO AFTER COMPLETION
			qStatus.onQuest = 1;
		}
		if (caller == "qNPC_I") {
			Debug.Log ("T");
			if (qStatus.onQuest == 1) {
				if (qStatus.qPhase [0] == 0) {
					//Issue quest 1:
					//ADD MOVEMENT SCRIPT PARAMETER TO HALT MOVEMENT UPON ENGAGEMENT OF QUEST
					//Messenger<bool>.Broadcast("qEngaged", true);
					qStatus.onQuest = 2;
					Messenger<int>.AddListener ("contBtnRtrn", compileDecision);

					Messenger<string, string, int>.AddListener ("Siebelq1Rtrn", limbo_compileDecision);
					Messenger<string, string, int>.AddListener ("SiebelGoodbye", limbo_compileDecision);
					StartCoroutine (firstStage (wasSuccessful => {
						if (wasSuccessful != null) {
							success_stgI = wasSuccessful;
						}
					}));
					Debug.Log ("Returned to start");
				}
				if (success_stgI) {
					Debug.Log ("Quest Successfully Completed");
					qStatus.onQuest = 0;
					Messenger<int>.RemoveListener ("contBtnRtrn", compileDecision);
					Messenger<string, string, int>.RemoveListener ("Siebelq1Rtrn", limbo_compileDecision);
					Messenger<string, string, int>.RemoveListener ("SiebelGoodbye", limbo_compileDecision);
				}
			}
		} else if (caller == "qNPC_II") {
			Debug.Log ("ran");
			if (qStatus.onQuest == 1) {
				if (/*qStatus.qPhase [0] == 2 &&*/ qStatus.qPhase [1] == 0) {
					//Issue quest 2:
					//ADD MOVEMENT SCRIPT PARAMETER TO HALT MOVEMENT UPON ENGAGEMENT OF QUEST
					//Messenger<bool>.Broadcast("qEngaged", true);
					qStatus.onQuest = 2;
					Messenger<int>.AddListener ("contBtnRtrn", compileDecision);

					Messenger<string, string, int>.AddListener ("Froschq2Rtrn", limbo_compileDecision);
					Messenger<string>.AddListener ("LyricEntered", updateLEResponse);
					Messenger<bool>.AddListener ("Chant1Finished", compileResponseII_i);

					StartCoroutine (secondStage (wasSuccessful => {
						if (wasSuccessful != null) {
							success_stgII = wasSuccessful;
						}
					}));
				}
			}
		} else if (caller == "qNPC_Tester") {
			Debug.Log ("Engaged Test NPC, NPC Inactive");
			qStatus.onQuest = 2;
			Messenger<int>.AddListener ("contBtnRtrn", compileDecision);
			//StartCoroutine (testerStage());
		}
	}

	//DIALOGUE TESTER STAGE **TEMPORARY**
	/*private IEnumerator testerStage(){
		//RELOCATE AS SCRIPT-WIDE VARIABLE
		Interactable dialogueProxy;
		Dictionary<string, string> dialogueRef = new Dictionary<string, string> ();
		string npcName = "Tester";
		string[] contOptions = new string[2];
		//Holder
		int qBranchI = -1;

		dialogueProxy = tDialogueCanvas.GetComponent<Interactable> ();
		dialogueRef = dialogueProxy.sentenceArchive_Tester;
		string OrationI = dialogueRef ["tDialogue"];
		contOptions = new string[]{ "Left (1)", "Right (2)" };
		dlgInterface.outputDialogue (npcName, OrationI, contOptions);

		inputReceived = false;
		inputReturn = -1;
		yield return new WaitUntil (() => inputReceived);
		qBranchI = inputReturn;

		string OrationII = "Holder";
		if (qBranchI == 0) {
			OrationII = dialogueRef ["tDialogue0"];
		} else if (qBranchI == 1) {
			OrationII = dialogueRef ["tDialogue1"];
		} else {
			Debug.Log ("Failure");
		}
		dlgInterface.outputDialogue (npcName, OrationII, contOptions);
		//Prompter.SetActive (true);
		/*
		GameObject DBOne = new GameObject ();
		StartCoroutine (createDB (1, DBGet => {
			if (DBGet != null) {
				DBOne = DBGet;
			}
		}));
		
		string OrationI = dialogueRef ["tDialogue"];
		Text DBOneTxt = DBOne.GetComponent<Text> ();
		DBOneTxt.text = "This is a test: " + OrationI;


		yield return null;
	}*/
	private IEnumerator displayResponse(Dictionary<string, string> responseArchive, string path){
		string[] options_Filler = new string[0];

		Debug.Log (path);
		string[] Response = new string[]{ responseArchive [path] };
		dlgInterface.outputDialogue ("Faust", Response, options_Filler);

		yield return new WaitForSeconds (5.5f);

		canContinue = true;
		yield return null;
	}

	private IEnumerator secondStage(Action<bool> Accomplished){
		Debug.Log ("Engaged Frosch");
		Dictionary<string, string> dialogueRef_NPC = dDictionaryArchive.sentences_Frosch;
		Dictionary<string, string> dialogueRef_User = dDictionaryArchive.responses_Frosch;
		string NPCname = "Frosch";
		string[] contOptions = new string[2];

		//bool canContinue = false;
		int qBranchI = -1;
		int qBranchII = -1;
		int qBranchIII = -1;
		bool songIAchieved = false;
		string branchPath = "fshDialogue";

		//Prompter.SetActive (true);

		string OrationI_a = dialogueRef_NPC [branchPath];
		string[] OrationI = new string[]{ OrationI_a };
		contOptions = new string[]{ "Mention Siebel", "Greet" };
		dlgInterface.outputDialogue (NPCname, OrationI, contOptions);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);
		//yield return new WaitForSeconds (2);

		qBranchI = inputReturn;
		branchPath = branchPath + qBranchI.ToString ();
		canContinue = false;
		Debug.Log (dialogueRef_User ["fshDialogue0"]);
		StartCoroutine (displayResponse (dialogueRef_User, branchPath));

		yield return new WaitUntil (() => canContinue);

		string OrationII_a = dialogueRef_NPC [branchPath + "a"];
		string OrationII_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationII = new string[]{ OrationII_a, OrationII_b };
		if (qBranchI == 0) {
			//Debug.Log ("Display NPC response for path route 0");
			contOptions = new string[]{ "Accept", "Decline" };
		} else if (qBranchI == 1) {
			//Debug.Log ("Display NPC response for path route 1");
			contOptions = new string[]{ "Make Small Talk", "Ask About Wine" };
		}
		dlgInterface.outputDialogue (NPCname, OrationII, contOptions);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchII = inputReturn;
		branchPath = branchPath + qBranchII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIII_a = "Holder";
		string OrationIII_b = "Holder";
		string[] OrationIII = new string[]{ };
		if (qBranchI == 0) {
			OrationIII = new string[]{ dialogueRef_NPC [branchPath] };
			if (qBranchII == 0) {
				contOptions = new string[]{ "Holder", "Holder" };
			} else if (qBranchII == 1) {
				contOptions = new string[]{ "Holder", "Holder" };
			}
		} else if(qBranchI == 1){
			OrationIII_a = dialogueRef_NPC [branchPath + "a"];
			OrationIII_b = dialogueRef_NPC [branchPath + "b"];
			OrationIII = new string[]{ OrationIII_a, OrationIII_b };
			if (qBranchII == 0) {
				contOptions = new string[]{ "Further Inquire", "Ask About Wine" };
			} else if (qBranchII == 1) {
				contOptions = new string[]{ "Mention Siebel", "Discuss Wine" };
			}
		}
		dlgInterface.outputDialogue (NPCname, OrationIII, contOptions);
		Debug.Log ("End Reached: Remainder Uncompleted");

		//*TEMPORARY*
		canContinue = false;
		yield return new WaitUntil (() => canContinue);

	}
	//CURRENTLY INACTIVE METHOD:
	private IEnumerator textCrawl(Dictionary<string, float> lyrics, Text DBTxt){
		string[] words = new string[19];
		float[] pauses = new float[19];
		lyrics.Keys.CopyTo (words, 0);
		lyrics.Values.CopyTo (pauses, 0);

		string Song_Reassembled = "";
		for (int i = 0; i < lyrics.Count; i++) {
			Song_Reassembled = Song_Reassembled + words [i];
			DBTxt.text = "Frosch: " + Song_Reassembled;

			yield return new WaitForSeconds (pauses [i]);
		}
		TCgate = true;
		yield return null;
	}

	private IEnumerator firstStage(Action<bool> Accomplished){
		Debug.Log ("Engaged Siebel");
		Dictionary<string, string> dialogueRef_NPC = dDictionaryArchive.sentences_Siebel;
		Dictionary<string, string> dialogueRef_User = dDictionaryArchive.responses_Siebel;
		string NPCname = "Siebel";
		string[] contOptions = new string[]{};

		//activeQNPC = Stage # - 1:
		activeQNPC = 0;
		qStatus.qPhase [activeQNPC] = 1;

		int qBranchI = -1;
		int qBranchII = -1;
		int qBranchIII = -1;
		int qBranchIV = -1;
		bool canReturn = false;
		bool qComplete = false;
		string branchPath = "sblDialogue";

		string[] OrationI = new string[]{ dialogueRef_NPC [branchPath] };
		contOptions = new string[]{ "Greet", "Dismiss" };
		dlgInterface.outputDialogue (NPCname, OrationI, contOptions);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);
		//yield return new WaitForSeconds (2);

		qBranchI = inputReturn;
		branchPath = branchPath + qBranchI.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationII_a = dialogueRef_NPC [branchPath + "a"];
		string OrationII_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationII = new string[]{ OrationII_a, OrationII_b };
		if (qBranchI == 0) {
			//Debug.Log ("Display NPC response for path route 0");
			contOptions = new string[]{ "Answer", "Change Topic" };
		} else if (qBranchI == 1) {
			//Debug.Log ("Display NPC response for path route 1");
			contOptions = new string[]{ "Answer", "Digress" };
		}
		dlgInterface.outputDialogue (NPCname, OrationII, contOptions);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchII = inputReturn;
		branchPath = branchPath + qBranchII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIII_a = "Holder";
		string OrationIII_b = "Holder";
		string[] OrationIII = new string[]{ };
		if (qBranchI == 0) {
			OrationIII_a = dialogueRef_NPC [branchPath + "a"];
			OrationIII_b = dialogueRef_NPC [branchPath + "b"];
			OrationIII = new string[]{ OrationIII_a, OrationIII_b };
			if (qBranchII == 0) {
				contOptions = new string[]{ "Accept Offer", "Decline Offer" };
			} else if (qBranchII == 1) {
				contOptions = new string[]{ "Ask Preferences", "Agree" };
			}
		} else if(qBranchI == 1){
			if (qBranchII == 0) {
				OrationIII = new string[]{ dialogueRef_NPC [branchPath] };
				contOptions = new string[]{ "Change Topic", "Attempt to Pander" };
			} else if (qBranchII == 1) {
				OrationIII_a = dialogueRef_NPC [branchPath + "a"];
				OrationIII_b = dialogueRef_NPC [branchPath + "b"];
				OrationIII = new string[]{ OrationIII_a, OrationIII_b };
				contOptions = new string[]{ "Ask Preferences", "Ask for Wine" };
			}
		}
		dlgInterface.outputDialogue (NPCname, OrationIII, contOptions);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIII = inputReturn;
		branchPath = branchPath + qBranchIII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIV_a = "Holder";
		string OrationIV_b = "Holder";
		string[] OrationIV = new string[]{ };
		if (qBranchI == 0) {
			if (qBranchII == 0) {
				OrationIV = new string[]{ dialogueRef_NPC [branchPath] };

			} else if (qBranchII == 1) {
				if (qBranchIII == 0) {
					OrationIV_a = dialogueRef_NPC [branchPath + "a"];
					OrationIV_b = dialogueRef_NPC [branchPath + "b"];
					OrationIV = new string[]{ OrationIV_a, OrationIV_b };
				} else if (qBranchIII == 1) {
					OrationIV = new string[]{ dialogueRef_NPC [branchPath] };
				}
			}
			contOptions = new string[]{ "Leave" };
			qComplete = true;
		} else if (qBranchI == 1) {
			if (qBranchII == 0) {
				OrationIV_a = dialogueRef_NPC [branchPath + "a"];
				OrationIV_b = dialogueRef_NPC [branchPath + "b"];
				OrationIV = new string[]{ OrationIV_a, OrationIV_b };
				contOptions = new string[]{ "Leave" };
				qComplete = true;
			} else if (qBranchII == 1) {
				if (qBranchIII == 0) {
					//CONTINUATION
					OrationIV_a = dialogueRef_NPC [branchPath + "a"];
					OrationIV_b = dialogueRef_NPC [branchPath + "b"];
					OrationIV = new string[]{ OrationIV_a, OrationIV_b };
					contOptions = new string[]{ "Holder", "Holder" };
				} else if (qBranchIII == 1) {
					OrationIV = new string[]{ dialogueRef_NPC [branchPath] };
					contOptions = new string[]{ "Leave" };
					qComplete = true;
				}
			}
		}
		dlgInterface.outputDialogue (NPCname, OrationIV, contOptions);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIV = inputReturn;
		if (qBranchIV == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			Accomplished (true);
			yield return null;
		} else {
			Debug.Log ("Continuing");
		}

		//*TEMPORARY*
		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		yield return null;
	}
	//CURRENTLY INACTIVE METHOD:
	private IEnumerator checkCompletion(bool completionRef, Action<bool> rtrnGate){
		rtrnGate (completionRef);
		if (completionRef) {
			yield return new WaitForSeconds (2f);
			//Set stage's qPhase to complete
			qStatus.qPhase [activeQNPC] = 2;
			//Removal of stage's events (Cleanup)
			GameObject[] activeDBs = GameObject.FindGameObjectsWithTag ("DBSlot");
			if (activeDBs.Length > 0) {
				foreach (GameObject DB in activeDBs) {
					Destroy (DB);
				}
			}
			Prompter.SetActive (false);
		}
		yield return null;
	}

	public void qPack_Mn(playerStatusLog pStatus){
		int tracker = 0;
		if (tracker == 0) {
			pStatus.qPhaseSet (stgRef);
			tracker++;
		}
	}
}