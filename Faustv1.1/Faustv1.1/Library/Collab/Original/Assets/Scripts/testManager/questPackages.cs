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
	private DialogueManager dlgInterface;
	private Interactable dDictionaryArchive;
	private GameObject dlgManager;
	private GameObject Prompter;
	private GameObject Player;
	private GameObject tDialogueCanvas;

	//REFORMAT
	private bool inputReceived;
	private bool canContinue;
	private bool success_stgI;
	private bool success_stgII;
	private bool DBgate;
	private bool FWgate;
	private int inputReturn;
	private List<int> stgRef;
	private int activeQNPC;
	private playerStatusLog qStatus;
	private Canvas dBCanvas;
	private Text tempTxt;
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

		qStatus = Player.GetComponent<playerStatusLog> ();
		qStatus.qPhaseSet (stgRef);
		canContinue = false;
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
		Debug.Log (inputReturn);
	}
	public void LEGate(bool LESuccess){
		inputReceived = true;
		if (LESuccess) {
			inputReturn = 1;
		} else if (!LESuccess) {
			inputReturn = 0;
		}
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

					//Messenger<string, string, int>.AddListener ("Siebelq1Rtrn", limbo_compileDecision);
					//Messenger<string, string, int>.AddListener ("SiebelGoodbye", limbo_compileDecision);
					StartCoroutine (firstStage (wasSuccessful => {
						if (wasSuccessful != null) {
							Debug.Log ("Returned to start");
							success_stgI = wasSuccessful;
						}
					}));
					if (success_stgI) {
						Debug.Log ("Quest Successfully Completed");
						qStatus.onQuest = 0;
						Messenger<int>.RemoveListener ("contBtnRtrn", compileDecision);
						//Messenger<string, string, int>.RemoveListener ("Siebelq1Rtrn", limbo_compileDecision);
						//Messenger<string, string, int>.RemoveListener ("SiebelGoodbye", limbo_compileDecision);
					}
				}
			}
		} else if (caller == "qNPC_II") {
			Debug.Log ("ran");
			if (qStatus.onQuest == 1) {
				Debug.Log ("Ran");
				if (/*qStatus.qPhase [0] == 2 &&*/ qStatus.qPhase [1] == 0) {
					Debug.Log ("Ran v2");
					//Issue quest 2:
					//ADD MOVEMENT SCRIPT PARAMETER TO HALT MOVEMENT UPON ENGAGEMENT OF QUEST
					qStatus.onQuest = 2;
					Messenger<int>.AddListener ("contBtnRtrn", compileDecision);
					Messenger<bool>.AddListener ("LERtrn", LEGate);

					StartCoroutine (secondStage (wasSuccessful => {
						if (wasSuccessful != null) {
							Debug.Log("Returned to start");
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
		

	private IEnumerator displayResponse(Dictionary<string, string> responseArchive, string path, int rdType){
		string[] options_Filler = new string[0];

		Debug.Log (path);
		string[] Response = new string[]{ responseArchive [path] };
		dlgInterface.outputDialogue ("Faust", rdType, Response, options_Filler, -1);

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
		//string[] buffer = new string[]{ };
		//bool canContinue = false;
		int qBranchI = -1;
		int qBranchII = -1;
		int qBranchIII = -1;
		int qBranchIV = -1;
		int qBranchV = -1;
		int qBranchVI = -1;
		int chantKeyHolder = -1;
		int chantKey = 0;
		string branchPath = "fshDialogue";


		string OrationI_a = dialogueRef_NPC [branchPath];
		string[] OrationI = new string[]{ OrationI_a };
		contOptions = new string[]{ "Mention Siebel", "Greet" };
		dlgInterface.outputDialogue (NPCname, 1, OrationI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);
		//yield return new WaitForSeconds (2);

		qBranchI = inputReturn;
		branchPath = branchPath + qBranchI.ToString ();
		canContinue = false;
		Debug.Log (dialogueRef_User ["fshDialogue0"]);
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

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
		dlgInterface.outputDialogue (NPCname, 1, OrationII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchII = inputReturn;
		branchPath = branchPath + qBranchII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIII_a = dialogueRef_NPC [branchPath + "a"];
		string OrationIII_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationIII = new string[]{ OrationIII_a, OrationIII_b };
		int rspnsTypeIII = 0;
		int rdTypeI = 0;
		if (qBranchI == 0) {
			rspnsTypeIII = 2;
			rdTypeI = -1;
			chantKey = 0;
		} else if(qBranchI == 1){
			rspnsTypeIII = 1;
			if (qBranchII == 0) {
				rdTypeI = 1;
				contOptions = new string[]{ "Further Inquire", "Ask About Wine" };
			} else if (qBranchII == 1) {
				rdTypeI = 1;
				contOptions = new string[]{ "Mention Siebel", "Discuss Wine" };
			}
		}
		dlgInterface.outputDialogue (NPCname, rspnsTypeIII, OrationIII, contOptions, chantKey);
		//Debug.Log ("End Reached: Remainder Uncompleted");

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIII = inputReturn; 
		if (qBranchI == 0 && qBranchIII == 0) {
			//Quest failed
			Debug.Log ("Failed LE; Quest failed");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			//Return to start:
			Accomplished (true);
			yield break;
		}
		branchPath = branchPath + qBranchIII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, rdTypeI));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIV_a = dialogueRef_NPC [branchPath + "a"];
		string OrationIV_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationIV = new string[]{ OrationIV_a, OrationIV_b };
		int rspnsTypeIV = 0;
		int rdTypeII = 0;
		if (qBranchI == 0) {
			if (qBranchIII == 1) {
				rdTypeII = -1;
				rspnsTypeIV = 2;
				chantKey = 1;
			}
		} else if (qBranchI == 1) {
			rspnsTypeIV = 1;
			rdTypeII = 1;
			if (qBranchII == 0) {
				if (qBranchIII == 0) {
					contOptions = new string[]{ "Decline", "Accept" };
				} else if (qBranchIII == 1) {
					//Completed
					contOptions = new string[]{ "Leave" };
				}
			} else if (qBranchII == 1) {
				//Completed
				contOptions = new string[]{ "Leave" };
			}
		}
		dlgInterface.outputDialogue (NPCname, rspnsTypeIV, OrationIV, contOptions, chantKey);
		
		
		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIV = inputReturn;
		Debug.Log (qBranchIV);
		if (qBranchIV == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			Accomplished (true);
			yield break;
		} else if (qBranchI == 0 && qBranchIV == 0) {
			//Quest failed
			Debug.Log ("Failed LE; Quest failed");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			//Return to start:
			Accomplished (true);
			yield break;
		}

		branchPath = branchPath + qBranchIV.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, rdTypeII));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationV_a = dialogueRef_NPC [branchPath + "a"];
		string OrationV_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationV = new string[]{ OrationV_a, OrationV_b };
		int rspnsTypeV = 0;
		int rdTypeIII = 0;
		if (qBranchI == 0) {
			if (qBranchIV == 1) {
				rdTypeIII = -1;
				rspnsTypeV = 2;
				chantKey = 2;
			}
		} else if (qBranchI == 1) {
			rspnsTypeV = 1;
			rdTypeIII = 1;
			if (qBranchIV == 0) {
				//Quest failed
				contOptions = new string[]{ "Leave" };
			} else if (qBranchIV == 1) {
				contOptions = new string[]{ "Leave" };
			}
		}
		dlgInterface.outputDialogue (NPCname, rspnsTypeV, OrationV, contOptions, chantKey);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchV = inputReturn;
		Debug.Log (qBranchV);
		if (qBranchV == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			Accomplished (true);
			yield break;
		} else if (qBranchV == 0) {
			//Quest failed
			Debug.Log ("Failed LE; Quest failed");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			//Return to start:
			Accomplished (true);
			yield break;
		}

		branchPath = branchPath + qBranchV.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, rdTypeIII));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationVI_a = dialogueRef_NPC [branchPath + "a"];
		string OrationVI_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationVI = new string[]{ OrationVI_a, OrationVI_b };
		//Differentiates between LE response and contButton response
		int rspnsTypeVI = 1;
		if (qBranchV == 1) {
			contOptions = new string[]{ "Leave" };
		} else {
			Debug.Log ("Error");
		}

		dlgInterface.outputDialogue (NPCname, rspnsTypeVI, OrationVI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchVI = inputReturn;
		Debug.Log (qBranchVI);
		if (qBranchVI == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [activeQNPC] = 2;

			Accomplished (true);
			yield break;
		} else {
			Debug.Log ("Error");
		}

		//*TEMPORARY*
		canContinue = false;
		yield return new WaitUntil (() => canContinue);

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
		int chantKeyHolder = -1;
		bool qComplete = false;
		string branchPath = "sblDialogue";

		string[] OrationI = new string[]{ dialogueRef_NPC [branchPath] };
		contOptions = new string[]{ "Greet", "Dismiss" };
		dlgInterface.outputDialogue (NPCname, 1, OrationI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);
		//yield return new WaitForSeconds (2);

		qBranchI = inputReturn;
		branchPath = branchPath + qBranchI.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

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
		dlgInterface.outputDialogue (NPCname, 1, OrationII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchII = inputReturn;
		branchPath = branchPath + qBranchII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

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
		dlgInterface.outputDialogue (NPCname, 1, OrationIII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIII = inputReturn;
		branchPath = branchPath + qBranchIII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

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
		dlgInterface.outputDialogue (NPCname, 1, OrationIV, contOptions, chantKeyHolder);

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